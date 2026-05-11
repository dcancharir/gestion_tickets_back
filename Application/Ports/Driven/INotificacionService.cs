using Application.DTOS.Notificaciones;

namespace Application.Ports.Driven;

public interface INotificacionService {
    Task NotificarUsuarioAsync(int usuarioId, NotificacionDto dto, CancellationToken ct = default);
}
