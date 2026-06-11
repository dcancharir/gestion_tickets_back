namespace Application.Ports.Driven;

public interface INotificacionService {
    /// <summary>
    /// Persiste la notificación en BD y la envía en tiempo real via SignalR.
    /// El DTO enviado incluye el NotificacionId asignado por la BD.
    /// </summary>
    /// <param name="referencia">Identificador visible al usuario (p.ej. "TK-001"). Null para notificaciones de sistema.</param>
    /// <param name="urlDestino">Ruta Angular destino del click (p.ej. "/tickets/{guid}"). Null = sin redirección.</param>
    Task GuardarYNotificarAsync(
        int     usuarioId,
        string  tipo,
        string  titulo,
        string  mensaje,
        string? referencia   = null,
        string? urlDestino   = null,
        CancellationToken ct = default);
}
