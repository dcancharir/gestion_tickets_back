using Application.DTOS.Reportes;

namespace Application.Ports.Driven;

public interface IReportesRepository {
    Task<ReportesDto>             ObtenerReportesAsync(DateTime desde, DateTime hasta, CancellationToken ct = default);
    Task<ReporteDistribucionDto>  ObtenerDistribucionAsync(CancellationToken ct = default);
}
