namespace Application.DTOS.Dashboard;

// ─────────────────────────────────────────────────────────────────────────────
// DTO raíz para el dashboard personalizado del Técnico
// ─────────────────────────────────────────────────────────────────────────────
public record DashboardKpiTecnicoDto(
    // Contadores personales de tickets
    ResumenTecnicoDto          MisTickets,

    // KPIs personales de rendimiento
    double?                    MiMttrMinutos,
    double?                    MiSla,
    double?                    MiPrimerContacto,

    // Tickets que requieren atención urgente
    IEnumerable<TicketResumenDto> ProximosAVencerSla,
    IEnumerable<TicketResumenDto> CriticosAbiertos
);

// Contadores de estado para el técnico autenticado
public record ResumenTecnicoDto(
    int TotalAsignados,
    int EnProgreso,
    int Pendientes,
    int ResueltosHoy,
    int Criticos          // tickets críticos aún no resueltos
);

// Ficha reducida de un ticket para las tablas de alertas
public record TicketResumenDto(
    string   PublicId,
    string   NumeroTicket,
    string   Titulo,
    string   Estado,
    string   Prioridad,
    DateTime FechaRegistro,
    DateTime? FechaLimiteResolucion
);
