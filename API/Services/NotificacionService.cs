using API.Hubs;
using Application.DTOS.Notificaciones;
using Application.Ports.Driven;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace API.Services;

/// <summary>
/// Implementación del servicio de notificaciones.
/// Persiste cada notificación en BD y luego la envía en tiempo real via SignalR.
/// Vive en la capa API porque depende de NotificacionHub (SignalR).
/// </summary>
public class NotificacionService : INotificacionService {
    private readonly IHubContext<NotificacionHub> _hub;
    private readonly INotificacionRepository _notifRepo;

    public NotificacionService(
        IHubContext<NotificacionHub> hub,
        INotificacionRepository notifRepo) {
        _hub       = hub;
        _notifRepo = notifRepo;
    }

    public async Task GuardarYNotificarAsync(
        int     usuarioId,
        string  tipo,
        string  titulo,
        string  mensaje,
        string? referencia   = null,
        string? urlDestino   = null,
        CancellationToken ct = default) {

        // 1. Guardar en BD para que el usuario la vea al (re)iniciar sesión
        var notificacion = await _notifRepo.CrearAsync(new Notificacion {
            UsuarioId     = usuarioId,
            Tipo          = tipo,
            Referencia    = referencia,
            Titulo        = titulo,
            Mensaje       = mensaje,
            UrlDestino    = urlDestino,
            FechaCreacion = DateTime.Now
        }, ct);

        // 2. Enviar en tiempo real por SignalR con el ID ya asignado por la BD
        await _hub.Clients
            .User(usuarioId.ToString())
            .SendAsync("RecibirNotificacion", new NotificacionDto(
                notificacion.NotificacionId,
                tipo,
                referencia,
                titulo,
                mensaje,
                Leida:      false,
                UrlDestino: urlDestino,
                notificacion.FechaCreacion
            ), ct);
    }
}
