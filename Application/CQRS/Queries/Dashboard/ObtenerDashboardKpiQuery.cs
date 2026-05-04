using Application.CQRS.Core;
using Application.DTOS.Dashboard;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Dashboard;

public record ObtenerDashboardKpiQuery() : IQuery<DashboardKpiDto>;
public class ObtenerDashboardKpiHandler
    : IQueryHandler<ObtenerDashboardKpiQuery, DashboardKpiDto> {
    private readonly IDashboardRepository _repo;

    public ObtenerDashboardKpiHandler(IDashboardRepository repo) => _repo = repo;

    public async Task<DashboardKpiDto> HandleAsync(
        ObtenerDashboardKpiQuery query,
        CancellationToken ct = default) {
        return await _repo.ObtenerKpisAsync(ct);
    }
}