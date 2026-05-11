namespace Application.DTOS.Notificaciones;

public record NotificacionDto(
    string Tipo,
    string TicketPublicId,
    string NumeroTicket,
    string Titulo,
    string Mensaje
);
