using Application.CQRS.Core;
using Application.CQRS.Queries.Dashboard;
using Application.DTOS.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase {
    private readonly IDispatcher _dispatcher;

    public DashboardController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    // GET api/dashboard/kpis
    // Admin y Técnico pueden ver el dashboard completo
    [HttpGet("kpis")]
    //[Authorize(Roles = "Administrador,Técnico")]
    [ProducesResponseType(typeof(DashboardKpiDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetKpis(CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(new ObtenerDashboardKpiQuery(), ct);
        return Ok(result);
    }
}
