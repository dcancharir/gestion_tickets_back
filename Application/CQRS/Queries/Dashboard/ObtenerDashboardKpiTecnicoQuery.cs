using Application.CQRS.Core;
using Application.DTOS.Dashboard;
using Application.Ports.Driven;

namespace Application.CQRS.Queries.Dashboard;

public record ObtenerDashboardKpiTecnicoQuery(int TecnicoId) : IQuery<DashboardKpiTecnicoDto>;

public class ObtenerDashboardKpiTecnicoHandler
    : IQueryHandler<ObtenerDashboardKpiTecnicoQuery, DashboardKpiTecnicoDto> {
    private readonly IDashboardRepository _repo;

    public ObtenerDashboardKpiTecnicoHandler(IDashboardRepository repo) => _repo = repo;

    public Task<DashboardKpiTecnicoDto> HandleAsync(
        ObtenerDashboardKpiTecnicoQuery query,
        CancellationToken ct = default)
        => _repo.ObtenerKpisTecnicoAsync(query.TecnicoId, ct);
}
