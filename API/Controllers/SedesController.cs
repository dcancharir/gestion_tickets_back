using Application.CQRS.Core;
using Application.CQRS.Queries.Sedes;
using Application.DTOS.Categorias;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SedesController : ControllerBase {
    private readonly IDispatcher _dispatcher;
    public SedesController(IDispatcher dispatcher) => _dispatcher = dispatcher;
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoriaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct = default) =>
        Ok(await _dispatcher.QueryAsync(new ObtenerSedesQuery(), ct));
}
