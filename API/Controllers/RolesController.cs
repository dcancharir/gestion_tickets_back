using Application.CQRS.Commands.Roles;
using Application.CQRS.Core;
using Application.CQRS.Queries.Roles;
using Application.DTOS.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase {
    private readonly IDispatcher _dispatcher;
    public RolesController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RolDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _dispatcher.QueryAsync(new ObtenerRolesQuery(), ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RolDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct) =>
        Ok(await _dispatcher.QueryAsync(new ObtenerRolPorIdQuery(id), ct));

    [HttpPost]
    [ProducesResponseType(typeof(RolDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CrearRolDto dto, CancellationToken ct) {
        var result = await _dispatcher.SendAsync(new CrearRolCommand(dto.Nombre, dto.Descripcion), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.RolId }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(RolDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] ActualizarRolDto dto, CancellationToken ct) =>
        Ok(await _dispatcher.SendAsync(new ActualizarRolCommand(id, dto.Nombre, dto.Descripcion), ct));

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct) {
        await _dispatcher.SendAsync(new EliminarRolCommand(id), ct);
        return NoContent();
    }
}
