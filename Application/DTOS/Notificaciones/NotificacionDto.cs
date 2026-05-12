namespace Application.DTOS.Notificaciones;

public record NotificacionDto(
    int    NotificacionId,
    string Tipo,
    string TicketPublicId,
    string NumeroTicket,
    string Titulo,
    string Mensaje,
    DateTime FechaCreacion
);
