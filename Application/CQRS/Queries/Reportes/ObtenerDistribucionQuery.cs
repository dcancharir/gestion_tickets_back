using Application.CQRS.Core;
using Application.DTOS.Reportes;
using Application.Ports.Driven;

namespace Application.CQRS.Queries.Reportes;

public record ObtenerDistribucionQuery : IQuery<ReporteDistribucionDto>;

public class ObtenerDistribucionHandler : IQueryHandler<ObtenerDistribucionQuery, ReporteDistribucionDto> {
    private readonly IReportesRepository _repo;
    public ObtenerDistribucionHandler(IReportesRepository repo) => _repo = repo;

    public Task<ReporteDistribucionDto> HandleAsync(ObtenerDistribucionQuery q, CancellationToken ct = default)
        => _repo.ObtenerDistribucionAsync(ct);
}
