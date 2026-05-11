using Application.CQRS.Commands.UsuarioSede;
using Application.CQRS.Core;
using Application.CQRS.Queries.UsuarioSede;
using Application.DTOS.Sedes;
using Application.DTOS.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioSedeController : ControllerBase {
    private readonly IDispatcher _dispatcher;
    public UsuarioSedeController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpGet("{publicId:guid}/sedes")]
    [ProducesResponseType(typeof(IEnumerable<SedeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSedes(Guid publicId, CancellationToken ct = default) =>
        Ok(await _dispatcher.QueryAsync(new ObtenerSedesPorUsuarioQuery(publicId), ct));

    [HttpGet("sedes/{sedeId:int}/usuarios")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsuariosPorSede(int sedeId, CancellationToken ct = default) =>
        Ok(await _dispatcher.QueryAsync(new ObtenerUsuariosPorSedeQuery(sedeId), ct));

    [HttpPost("{publicId:guid}/sedes")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AsignarSedes(Guid publicId, [FromBody] List<int> sedeIds, CancellationToken ct = default) {
        await _dispatcher.SendAsync(new AsignarSedesUsuarioCommand(publicId, sedeIds), ct);
        return NoContent();
    }
}
