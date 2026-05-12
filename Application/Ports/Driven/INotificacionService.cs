namespace Application.Ports.Driven;

public interface INotificacionService {
    /// <summary>
    /// Persiste la notificación en BD y la envía en tiempo real via SignalR.
    /// El DTO enviado incluye el NotificacionId asignado por la BD.
    /// </summary>
    Task GuardarYNotificarAsync(
        int    usuarioId,
        string tipo,
        string ticketPublicId,
        string numeroTicket,
        string titulo,
        string mensaje,
        CancellationToken ct = default);
}
