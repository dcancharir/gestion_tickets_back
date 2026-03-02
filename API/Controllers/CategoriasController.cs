using Application.CQRS.Commands.Categorias;
using Application.CQRS.Core;
using Application.CQRS.Queries.Categorias;
using Application.DTOS.Categorias;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase {
    private readonly IDispatcher _dispatcher;
    public CategoriasController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    // GET api/categorias?soloActivas=true
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoriaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool soloActivas = false, CancellationToken ct = default) =>
        Ok(await _dispatcher.QueryAsync(new ObtenerCategoriasQuery(soloActivas), ct));

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct) =>
        Ok(await _dispatcher.QueryAsync(new ObtenerCategoriaPorIdQuery(id), ct));

    [HttpPost]
    [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CrearCategoriaDto dto, CancellationToken ct) {
        var result = await _dispatcher.SendAsync(new CrearCategoriaCommand(dto.Nombre, dto.Descripcion), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.CategoriaId }, result);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] ActualizarCategoriaDto dto, CancellationToken ct) =>
        Ok(await _dispatcher.SendAsync(
            new ActualizarCategoriaCommand(id, dto.Nombre, dto.Descripcion, dto.Activo), ct));

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct) {
        await _dispatcher.SendAsync(new EliminarCategoriaCommand(id), ct);
        return NoContent();
    }
}

