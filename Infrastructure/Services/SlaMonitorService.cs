using Application.Ports.Driven;
using Application.Utilities;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

// ─────────────────────────────────────────────────────────────────────────────
// SLA MONITOR SERVICE
// Servicio en segundo plano que corre cada 15 minutos.
// Detecta tickets cuyo SLA de resolución venció y que aún no fueron escalados.
//
// Lógica de notificación:
//   - Ticket CON técnico asignado → notifica al técnico (SignalR + email)
//   - Ticket SIN técnico asignado → notifica a todos los admins (RolId = 1)
//     para que asignen el ticket urgentemente.
// ─────────────────────────────────────────────────────────────────────────────

public class SlaMonitorService : BackgroundService {
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SlaMonitorService> _logger;

    private static readonly TimeSpan _intervalo = TimeSpan.FromMinutes(15);

    public SlaMonitorService(
        IServiceScopeFactory scopeFactory,
        ILogger<SlaMonitorService> logger) {
        _scopeFactory = scopeFactory;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        _logger.LogInformation("[SlaMonitor] Servicio iniciado — revisión cada {min} minutos.",
            _intervalo.TotalMinutes);

        // Pequeño retardo inicial para no interferir con el arranque de la API
        await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

        while(!stoppingToken.IsCancellationRequested) {
            try {
                await ProcesarEscalamientosAsync(stoppingToken);
            } catch(Exception ex) when(ex is not OperationCanceledException) {
                _logger.LogError(ex, "[SlaMonitor] Error inesperado durante la revisión de SLAs.");
            }

            await Task.Delay(_intervalo, stoppingToken);
        }

        _logger.LogInformation("[SlaMonitor] Servicio detenido.");
    }

    private async Task ProcesarEscalamientosAsync(CancellationToken ct) {
        // Repositorios y servicios son Scoped → un scope por ejecución
        using var scope    = _scopeFactory.CreateScope();
        var repo           = scope.ServiceProvider.GetRequiredService<IIncidenciaRepository>();
        var usuarioRepo    = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();
        var emailSvc       = scope.ServiceProvider.GetRequiredService<IEmailService>();
        var notifSvc       = scope.ServiceProvider.GetRequiredService<INotificacionService>();
        var appSettings    = scope.ServiceProvider.GetRequiredService<IAppSettings>();

        var vencidas = (await repo.ObtenerVencidasSinEscalarAsync(ct)).ToList();

        if(!vencidas.Any()) {
            _logger.LogInformation("[SlaMonitor] Sin tickets vencidos en esta revisión.");
            return;
        }

        _logger.LogWarning("[SlaMonitor] {count} ticket(s) con SLA vencido encontrados.", vencidas.Count);

        // Cargamos admins una sola vez para todos los tickets sin técnico
        // (evita una consulta a BD por cada ticket sin asignar)
        List<Usuario>? admins = null;

        var ahora = DateTime.Now;

        foreach(var incidencia in vencidas) {
            try {
                // 1) Marcar fecha de escalamiento — previene doble notificación
                await repo.EscalarAsync(incidencia.IncidenciaId, ahora, ct);

                // 2) Registrar en historial para trazabilidad ITIL
                var actorId = incidencia.TecnicoAsignadoId ?? incidencia.SolicitanteId;
                await repo.AgregarHistorialAsync(new HistorialIncidencia {
                    IncidenciaId   = incidencia.IncidenciaId,
                    UsuarioId      = actorId,
                    Accion         = "Escalamiento automático",
                    EstadoAnterior = incidencia.EstadoIncidencia?.Nombre,
                    EstadoNuevo    = incidencia.EstadoIncidencia?.Nombre,
                    Detalle        = $"SLA de resolución incumplido. Límite: " +
                                     $"{incidencia.FechaLimiteResolucion:yyyy-MM-dd HH:mm}. " +
                                     $"Escalamiento registrado automáticamente por el sistema."
                }, ct);

                var urlTicket = $"{appSettings.FrontendUrl}/tickets/{incidencia.PublicId}";

                // 3a) Ticket CON técnico asignado → notificar al técnico
                if(incidencia.TecnicoAsignadoId.HasValue && incidencia.TecnicoAsignado is not null) {
                    var tecnico = incidencia.TecnicoAsignado;

                    await notifSvc.GuardarYNotificarAsync(
                        tecnico.UsuarioId,
                        "SLA Incumplido",
                        incidencia.PublicId.ToString(),
                        incidencia.NumeroTicket,
                        incidencia.Titulo,
                        $"⚠️ El ticket {incidencia.NumeroTicket} ha superado su SLA " +
                        $"de resolución. Atención urgente requerida.",
                        ct);

                    var template = EmailTemplateStrings.SlaIncumplidoTemplate(
                        $"{tecnico.Nombre} {tecnico.Apellidos}",
                        incidencia.NumeroTicket,
                        incidencia.Titulo,
                        incidencia.FechaLimiteResolucion!.Value,
                        urlTicket);
                    _ = emailSvc.SendEmail(
                        tecnico.Email,
                        $"[{incidencia.NumeroTicket}] ⚠️ SLA incumplido — Acción requerida",
                        template,
                        true);

                    _logger.LogWarning(
                        "[SlaMonitor] Ticket {ticket} escalado → técnico notificado: {tecnico}.",
                        incidencia.NumeroTicket,
                        $"{tecnico.Nombre} {tecnico.Apellidos}");

                // 3b) Ticket SIN técnico → notificar a todos los admins
                } else {
                    // Carga lazy: solo consulta admins si hay al menos un ticket sin asignar
                    admins ??= (await usuarioRepo.ObtenerPorRolId(1)).ToList();

                    foreach(var admin in admins) {
                        await notifSvc.GuardarYNotificarAsync(
                            admin.UsuarioId,
                            "SLA Incumplido",
                            incidencia.PublicId.ToString(),
                            incidencia.NumeroTicket,
                            incidencia.Titulo,
                            $"⚠️ El ticket {incidencia.NumeroTicket} venció su SLA " +
                            $"y no tiene técnico asignado. Asignación urgente requerida.",
                            ct);

                        var template = EmailTemplateStrings.SlaIncumplidoSinTecnicoTemplate(
                            $"{admin.Nombre} {admin.Apellidos}",
                            incidencia.NumeroTicket,
                            incidencia.Titulo,
                            incidencia.FechaLimiteResolucion!.Value,
                            urlTicket);
                        _ = emailSvc.SendEmail(
                            admin.Email,
                            $"[{incidencia.NumeroTicket}] ⚠️ SLA vencido sin técnico asignado",
                            template,
                            true);
                    }

                    _logger.LogWarning(
                        "[SlaMonitor] Ticket {ticket} escalado → sin técnico, {n} admin(s) notificado(s).",
                        incidencia.NumeroTicket,
                        admins.Count);
                }

            } catch(Exception ex) {
                _logger.LogError(ex,
                    "[SlaMonitor] Error al procesar escalamiento del ticket {ticket}.",
                    incidencia.NumeroTicket);
            }
        }
    }
}
