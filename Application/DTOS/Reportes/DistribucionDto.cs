namespace Application.DTOS.Reportes;

public record TicketPorTecnicoDto(
    int    TecnicoId,
    string NombreTecnico,
    int    Total,
    int    Resueltos,
    int    Cerrados,
    int    Pendientes,
    double? TiempoPromedioResolucionMinutos
);

public record TicketPorSedeDto(
    int    SedeId,
    string NombreSede,
    string TipoSede,
    int    Total,
    int    Resueltos,
    int    Cerrados,
    int    Pendientes
);

public record ReporteDistribucionDto(
    IReadOnlyList<TicketPorTecnicoDto> PorTecnico,
    IReadOnlyList<TicketPorSedeDto>    PorSede
);
