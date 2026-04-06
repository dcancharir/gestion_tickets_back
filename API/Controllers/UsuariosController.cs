using Application.CQRS.Commands.Usuarios;
using Application.CQRS.Core;
using Application.CQRS.Queries.Usuarios;
using Application.DTOS.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase {
    private readonly IDispatcher _dispatcher;

    public UsuariosController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    // GET api/usuarios
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(new ObtenerUsuariosQuery(), ct);
        return Ok(result);
    }

    // GET api/usuarios/3fa85f64-5717-4562-b3fc-2c963f66afa6
    // La ruta usa Guid, nunca un int secuencial
    [HttpGet("{publicId:guid}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid publicId, CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(
            new ObtenerUsuarioPorPublicIdQuery(publicId), ct);
        return Ok(result);
    }

    // POST api/usuarios
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CrearUsuarioDto dto, CancellationToken ct) {
        var command = new CrearUsuarioCommand(
            dto.Nombre, dto.Apellidos, dto.Email, dto.RolId,dto.UserName);

        var result = await _dispatcher.SendAsync(command, ct);

        // La URL de retorno usa el PublicId (Guid), no el int
        return CreatedAtAction(nameof(GetById), new { publicId = result.PublicId }, result);
    }

    // PUT api/usuarios/3fa85f64-5717-4562-b3fc-2c963f66afa6
    [HttpPut("{publicId:guid}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        Guid publicId,
        [FromBody] ActualizarUsuarioDto dto,
        CancellationToken ct) {
        var command = new ActualizarUsuarioCommand(
            publicId, dto.Nombre, dto.Apellidos, dto.Email, dto.RolId, dto.Activo);

        var result = await _dispatcher.SendAsync(command, ct);
        return Ok(result);
    }

    // DELETE api/usuarios/3fa85f64-5717-4562-b3fc-2c963f66afa6
    [HttpDelete("{publicId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid publicId, CancellationToken ct) {
        await _dispatcher.SendAsync(new EliminarUsuarioCommand(publicId), ct);
        return NoContent();
    }
}
