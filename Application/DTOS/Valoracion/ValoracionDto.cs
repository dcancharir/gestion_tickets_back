namespace Application.DTOS.Valoracion;

public record ValoracionDto(
    int      ValoracionId,
    byte     Puntuacion,
    string?  Comentario,
    DateTime FechaValoracion
);

public record CrearValoracionDto(
    byte    Puntuacion,
    string? Comentario
);
