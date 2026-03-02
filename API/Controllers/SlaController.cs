using Application.CQRS.Commands.SLAs;
using Application.CQRS.Core;
using Application.CQRS.Queries.SLAs;
using Application.DTOS.SLAs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SlaController : ControllerBase {
    private readonly IDispatcher _dispatcher;
    public SlaController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SlaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _dispatcher.QueryAsync(new ObtenerSlasQuery(), ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(SlaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct) =>
        Ok(await _dispatcher.QueryAsync(new ObtenerSlaPorIdQuery(id), ct));

    [HttpPost]
    [ProducesResponseType(typeof(SlaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CrearSlaDto dto, CancellationToken ct) {
        var result = await _dispatcher.SendAsync(
            new CrearSlaCommand(dto.CategoriaId, dto.PrioridadId, dto.TiempoRespuestaMin, dto.TiempoResolucionMin), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.SlaId }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(SlaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] ActualizarSlaDto dto, CancellationToken ct) =>
        Ok(await _dispatcher.SendAsync(
            new ActualizarSlaCommand(id, dto.CategoriaId, dto.PrioridadId, dto.TiempoRespuestaMin, dto.TiempoResolucionMin, dto.Activo), ct));

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct) {
        await _dispatcher.SendAsync(new EliminarSlaCommand(id), ct);
        return NoContent();
    }
}

