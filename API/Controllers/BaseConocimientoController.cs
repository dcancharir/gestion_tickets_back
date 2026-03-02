using Application.CQRS.Commands.BaseConocimientos;
using Application.CQRS.Core;
using Application.CQRS.Queries.BaseConocimientos;
using Application.DTOS.BaseConocimiento;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/base-conocimiento")]
public class BaseConocimientoController : ControllerBase {
    private readonly IDispatcher _dispatcher;
    public BaseConocimientoController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    private int GetUsuarioId() =>
        int.Parse(Request.Headers["X-Usuario-Id"].FirstOrDefault() ?? "0");

    // GET api/base-conocimiento
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ArticuloListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool soloActivos = true, CancellationToken ct = default) {
        var result = await _dispatcher.QueryAsync(new ObtenerArticulosQuery(soloActivos), ct);
        return Ok(result);
    }

    // GET api/base-conocimiento/{publicId}
    [HttpGet("{publicId:guid}")]
    [ProducesResponseType(typeof(ArticuloDetalleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid publicId, CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(
            new ObtenerArticuloPorPublicIdQuery(publicId), ct);
        return Ok(result);
    }

    // GET api/base-conocimiento/buscar?termino=antivirus
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<ArticuloListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Buscar([FromQuery] string termino, CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(new BuscarArticulosQuery(termino), ct);
        return Ok(result);
    }

    // GET api/base-conocimiento/categoria/3
    [HttpGet("categoria/{categoriaId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ArticuloListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCategoria(int categoriaId, CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(
            new ObtenerArticulosPorCategoriaQuery(categoriaId), ct);
        return Ok(result);
    }

    // POST api/base-conocimiento
    [HttpPost]
    [ProducesResponseType(typeof(ArticuloDetalleDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CrearArticuloDto dto, CancellationToken ct) {
        var command = new CrearArticuloCommand(
            dto.Titulo, dto.Problema, dto.Solucion, dto.CategoriaId, GetUsuarioId());

        var result = await _dispatcher.SendAsync(command, ct);
        return CreatedAtAction(nameof(GetById), new { publicId = result.PublicId }, result);
    }

    // PUT api/base-conocimiento/{publicId}
    [HttpPut("{publicId:guid}")]
    [ProducesResponseType(typeof(ArticuloDetalleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid publicId, [FromBody] ActualizarArticuloDto dto, CancellationToken ct) {
        var command = new ActualizarArticuloCommand(
            publicId, dto.Titulo, dto.Problema, dto.Solucion, dto.CategoriaId, dto.Activo);

        var result = await _dispatcher.SendAsync(command, ct);
        return Ok(result);
    }

    // DELETE api/base-conocimiento/{publicId}
    [HttpDelete("{publicId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid publicId, CancellationToken ct) {
        await _dispatcher.SendAsync(new EliminarArticuloCommand(publicId), ct);
        return NoContent();
    }
}
