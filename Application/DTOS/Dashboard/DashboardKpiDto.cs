using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Dashboard;

public record DashboardKpiDto(
    // Contadores por estado
    ResumenEstadosDto Estados,

    // KPIs ITIL principales
    KpisItilDto KpisItil,

    // Distribución por categoría
    IEnumerable<ConteoDto> PorCategoria,

    // Distribución por prioridad
    IEnumerable<ConteoDto> PorPrioridad,

    // Top técnicos por tickets resueltos
    IEnumerable<KpiTecnicoDto> TopTecnicos
);
