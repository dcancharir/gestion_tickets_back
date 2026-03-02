using Application.CQRS.Commands.Configuracion;
using Application.CQRS.Core;
using Application.CQRS.Queries.Configuracion;
using Application.DTOS.Configuracion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/configuracion")]
public class ConfiguracionController : ControllerBase {
    private readonly IDispatcher _dispatcher;
    public ConfiguracionController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    // ── Niveles de Prioridad: GET /api/configuracion/prioridades ─────────────

    [HttpGet("prioridades")]
    [ProducesResponseType(typeof(IEnumerable<NivelPrioridadDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPrioridades(CancellationToken ct) =>
        Ok(await _dispatcher.QueryAsync(new ObtenerNivelesPrioridadQuery(), ct));

    [HttpPost("prioridades")]
    [ProducesResponseType(typeof(NivelPrioridadDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePrioridad([FromBody] CrearNivelPrioridadDto dto, CancellationToken ct) {
        var result = await _dispatcher.SendAsync(
            new CrearNivelPrioridadCommand(dto.Nombre, dto.Nivel, dto.TiempoRespuestaMin, dto.TiempoResolucionMin), ct);
        return Created($"api/configuracion/prioridades/{result.PrioridadId}", result);
    }

    [HttpPut("prioridades/{id:int}")]
    [ProducesResponseType(typeof(NivelPrioridadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePrioridad(int id, [FromBody] ActualizarNivelPrioridadDto dto, CancellationToken ct) =>
        Ok(await _dispatcher.SendAsync(
            new ActualizarNivelPrioridadCommand(id, dto.Nombre, dto.Nivel, dto.TiempoRespuestaMin, dto.TiempoResolucionMin), ct));

    [HttpDelete("prioridades/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePrioridad(int id, CancellationToken ct) {
        await _dispatcher.SendAsync(new EliminarNivelPrioridadCommand(id), ct);
        return NoContent();
    }

    // ── Estados de Incidencia: GET /api/configuracion/estados ────────────────

    [HttpGet("estados")]
    [ProducesResponseType(typeof(IEnumerable<EstadoIncidenciaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEstados(CancellationToken ct) =>
        Ok(await _dispatcher.QueryAsync(new ObtenerEstadosIncidenciaQuery(), ct));

    [HttpPost("estados")]
    [ProducesResponseType(typeof(EstadoIncidenciaDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateEstado([FromBody] CrearEstadoDto dto, CancellationToken ct) {
        var result = await _dispatcher.SendAsync(
            new CrearEstadoCommand(dto.Nombre, dto.Descripcion, dto.EsEstadoFinal), ct);
        return Created($"api/configuracion/estados/{result.EstadoId}", result);
    }

    [HttpPut("estados/{id:int}")]
    [ProducesResponseType(typeof(EstadoIncidenciaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEstado(int id, [FromBody] ActualizarEstadoDto dto, CancellationToken ct) =>
        Ok(await _dispatcher.SendAsync(
            new ActualizarEstadoCommand(id, dto.Nombre, dto.Descripcion, dto.EsEstadoFinal), ct));

    [HttpDelete("estados/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEstado(int id, CancellationToken ct) {
        await _dispatcher.SendAsync(new EliminarEstadoCommand(id), ct);
        return NoContent();
    }
}