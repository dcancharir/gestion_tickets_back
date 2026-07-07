namespace Application.DTOS.Reportes;

public record ReportesDto(
    // Contexto general del período
    int    TotalTickets,
    int    TicketsResueltos,
    int    TicketsCerrados,

    // Dimensión 1 — Satisfacción con el tiempo de atención
    double? TiempoPromedioRespuestaMinutos,
    double? TiempoPromedioResolucionMinutos,
    double? PorcentajeCumplimientoSla,

    // Dimensión 2 — Satisfacción con la solución brindada
    double? ValoracionPromedio,
    double? TasaReaperturasPct,

    // Dimensión 3 — Satisfacción con la comunicación y seguimiento
    double? PromedioComentariosPorTicket,
    double? PromedioActualizacionesHistorialPorTicket
);
