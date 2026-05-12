using Application.DTOS.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface IDashboardRepository {
    Task<DashboardKpiDto>        ObtenerKpisAsync(CancellationToken ct = default);
    Task<DashboardKpiTecnicoDto> ObtenerKpisTecnicoAsync(int tecnicoId, CancellationToken ct = default);
}
