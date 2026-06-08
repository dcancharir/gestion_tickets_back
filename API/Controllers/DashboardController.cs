using Application.CQRS.Core;
using Application.CQRS.Queries.Dashboard;
using Application.DTOS.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase {
    private readonly IDispatcher _dispatcher;

    public DashboardController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    // GET api/dashboard/kpis
    // Vista global para Administradores
    [HttpGet("kpis")]
    [ProducesResponseType(typeof(DashboardKpiDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetKpis(CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(new ObtenerDashboardKpiQuery(), ct);
        return Ok(result);
    }

    // GET api/dashboard/kpis-tecnico
    // Vista personal para el Técnico autenticado (lee su ID del JWT)
    [HttpGet("kpis-tecnico")]
    [ProducesResponseType(typeof(DashboardKpiTecnicoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetKpisTecnico(CancellationToken ct) {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(idClaim is null || !int.TryParse(idClaim, out var tecnicoId))
            return Unauthorized();

        var result = await _dispatcher.QueryAsync(
            new ObtenerDashboardKpiTecnicoQuery(tecnicoId), ct);
        return Ok(result);
    }

    // GET api/dashboard/tendencia-7d
    // Registrados y resueltos por día en los últimos 7 días
    [HttpGet("tendencia-7d")]
    [ProducesResponseType(typeof(IEnumerable<TendenciaDiaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTendencia7d(CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(new ObtenerTendencia7dQuery(), ct);
        return Ok(result);
    }
}
