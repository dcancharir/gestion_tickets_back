using Application.CQRS.Core;
using Application.DTOS.Reportes;
using Application.Ports.Driven;

namespace Application.CQRS.Queries.Reportes;

public record ObtenerReportesQuery(DateTime Desde, DateTime Hasta) : IQuery<ReportesDto>;

public class ObtenerReportesHandler : IQueryHandler<ObtenerReportesQuery, ReportesDto> {
    private readonly IReportesRepository _repo;
    public ObtenerReportesHandler(IReportesRepository repo) => _repo = repo;

    public Task<ReportesDto> HandleAsync(ObtenerReportesQuery q, CancellationToken ct = default)
        => _repo.ObtenerReportesAsync(q.Desde, q.Hasta, ct);
}
