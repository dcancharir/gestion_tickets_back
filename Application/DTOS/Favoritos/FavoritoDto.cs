namespace Application.DTOS.Favoritos;

public record FavoritoDto(
    int    IncidenciaId,
    Guid   PublicId,
    string NumeroTicket,
    string Titulo,
    string Descripcion,
    int    CategoriaId,
    string CategoriaNombre,
    string CanalReporte,
    byte   Impacto,
    byte   Urgencia,
    int    SedeId
);
