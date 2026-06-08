using Application.CQRS.Core;
using Application.DTOS.Dashboard;
using Application.Ports.Driven;

namespace Application.CQRS.Queries.Dashboard;

public record ObtenerTendencia7dQuery() : IQuery<IEnumerable<TendenciaDiaDto>>;

public class ObtenerTendencia7dHandler
    : IQueryHandler<ObtenerTendencia7dQuery, IEnumerable<TendenciaDiaDto>> {

    private readonly IDashboardRepository _repo;

    public ObtenerTendencia7dHandler(IDashboardRepository repo) => _repo = repo;

    public async Task<IEnumerable<TendenciaDiaDto>> HandleAsync(
        ObtenerTendencia7dQuery query,
        CancellationToken ct = default)
        => await _repo.ObtenerTendencia7dAsync(ct);
}
