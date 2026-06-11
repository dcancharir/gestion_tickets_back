namespace Application.DTOS.Notificaciones;

public record NotificacionDto(
    int      NotificacionId,
    string   Tipo,
    string?  Referencia,    // identificador visible (ticket#, artículo#…); null para notificaciones de sistema
    string   Titulo,
    string   Mensaje,
    bool     Leida,
    string?  UrlDestino,    // ruta Angular destino del click; null = sin redirección
    DateTime FechaCreacion
);
