using Domain.Entities;

namespace Application.Ports.Driven;

public interface INotificacionRepository {
    Task<Notificacion> CrearAsync(Notificacion notificacion, CancellationToken ct = default);
    Task<IEnumerable<Notificacion>> ObtenerNoLeidasPorUsuarioAsync(int usuarioId, CancellationToken ct = default);
    Task MarcarComoLeidaAsync(int notificacionId, int usuarioId, CancellationToken ct = default);
    Task MarcarTodasLeidasAsync(int usuarioId, CancellationToken ct = default);
}
