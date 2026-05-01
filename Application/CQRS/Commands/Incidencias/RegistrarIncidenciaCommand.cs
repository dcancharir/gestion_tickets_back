using Application.CQRS.Core;
using Application.CQRS.Queries.Incidencias;
using Application.DTOS.Incidencias;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Application.Utilities;

namespace Application.CQRS.Commands.Incidencias;

// ─────────────────────────────────────────────────────────────────────────────
// 1. REGISTRAR INCIDENCIA
// Cualquier rol puede crear. SolicitanteId viene del JWT.
// ─────────────────────────────────────────────────────────────────────────────

public record RegistrarIncidenciaCommand(
    string Titulo,
    string Descripcion,
    int CategoriaId,
    string CanalReporte,
    byte Impacto,
    byte Urgencia,
    int PrioridadId,
    int SolicitanteId,// resuelto desde el JWT en el controller
    int SedeId,
    List<IFormFile>? Adjuntos
) : ICommand<IncidenciaListItemDto>;

public class RegistrarIncidenciaHandler
    : ICommandHandler<RegistrarIncidenciaCommand, IncidenciaListItemDto> {
    private readonly IIncidenciaRepository _repo;
    private readonly IFileStorageService _fileStorageService;
    private readonly IIncidenciaAdjuntoRepository _adjuntoRepository;
    private readonly IEmailService _emailService;
    public RegistrarIncidenciaHandler(IIncidenciaRepository repo, IFileStorageService fileStorageService, IIncidenciaAdjuntoRepository adjuntoRepository, IEmailService emailService) {
        _repo = repo;
        _fileStorageService = fileStorageService;
        _adjuntoRepository = adjuntoRepository;
        _emailService = emailService;
    }

    public async Task<IncidenciaListItemDto> HandleAsync(
        RegistrarIncidenciaCommand cmd, CancellationToken ct = default) {
        // Generar número de ticket: TKT-YYYY-NNNNN
        var conteo = await _repo.ContarPorEstadoAsync(0, ct); // total general
        var numeroTicket = $"TKT-{DateTime.Now.Year}-{(conteo + 1):D5}";
        var ahora = DateTime.Now;

        // Calcular fechas límite SLA según prioridad
        // (los minutos vienen de NivelPrioridad, ya definidos en el seed)
        var incidencia = new Incidencia {
            NumeroTicket = numeroTicket,
            Titulo = cmd.Titulo,
            Descripcion = cmd.Descripcion,
            CategoriaId = cmd.CategoriaId,
            CanalReporte = cmd.CanalReporte,
            Impacto = cmd.Impacto,
            Urgencia = cmd.Urgencia,
            PrioridadId = cmd.PrioridadId,
            SolicitanteId = cmd.SolicitanteId,
            EstadoId = 1, // Registrado
            FechaRegistro = ahora,
            FechaUltimaActualizacion = ahora,
            SedeId = cmd.SedeId,
        };

        var creada = await _repo.CrearAsync(incidencia, ct);

        if(cmd.Adjuntos is not null && cmd.Adjuntos.Count()>0) {
            foreach(var file in cmd.Adjuntos) {
                var ruta = await _fileStorageService.SaveAsync(file, "incidencias");

                await _adjuntoRepository.CrearAsync(new IncidenciaAdjunto {
                    IncidenciaId = creada.IncidenciaId,
                    Nombre = Path.GetFileNameWithoutExtension(file.FileName),
                    NombreReal = file.FileName,
                    RutaContenedora = ruta,
                    FechaCreacion = DateTime.UtcNow
                }, ct);
            }
        }
        
        // Registrar en historial
        await _repo.AgregarHistorialAsync(new HistorialIncidencia {
            IncidenciaId = creada.IncidenciaId,
            UsuarioId = cmd.SolicitanteId,
            Accion = "Registro",
            EstadoNuevo = "Registrado",
            Detalle = $"Ticket {numeroTicket} registrado vía {cmd.CanalReporte}"
        }, ct);

        var completa = await _repo.ObtenerPorIdAsync(creada.IncidenciaId, ct);
        var mapped = IncidenciaMapper.ToListItem(completa!);
        var template = EmailTemplateStrings.NewIncidenciaTemplate(mapped, $"http://localhost:4200/principal/ticket-detail/{mapped.PublicId}");
        _ = _emailService.SendEmail("diego.canchari@designdevsoftware.com","Sistema de Gestión de Incidencias - Nueva Incidencia",template,true);
        return mapped;
    }
}
