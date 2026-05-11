using API.Hubs;
using Application.DTOS.Notificaciones;
using Application.Ports.Driven;
using Microsoft.AspNetCore.SignalR;

namespace API.Services;

/// <summary>
/// Implementación del servicio de notificaciones en tiempo real via SignalR.
/// Vive en la capa API porque depende directamente de NotificacionHub.
/// </summary>
public class NotificacionService : INotificacionService {
    private readonly IHubContext<NotificacionHub> _hub;

    public NotificacionService(IHubContext<NotificacionHub> hub) {
        _hub = hub;
    }

    public async Task NotificarUsuarioAsync(int usuarioId, NotificacionDto dto, CancellationToken ct = default) {
        // IUserIdProvider usa ClaimTypes.NameIdentifier → usuario.UsuarioId.ToString()
        await _hub.Clients
            .User(usuarioId.ToString())
            .SendAsync("RecibirNotificacion", dto, ct);
    }
}
