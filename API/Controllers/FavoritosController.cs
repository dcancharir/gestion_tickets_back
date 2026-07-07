using Application.CQRS.Core;
using Application.CQRS.Queries.Favoritos;
using Application.DTOS.Favoritos;
using Application.Ports.Driven;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FavoritosController : ControllerBase {
    private readonly IDispatcher          _dispatcher;
    private readonly IFavoritoRepository  _repo;

    public FavoritosController(IDispatcher dispatcher, IFavoritoRepository repo) {
        _dispatcher = dispatcher;
        _repo       = repo;
    }

    private int UserId =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET api/favoritos
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<FavoritoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(new ObtenerFavoritosQuery(UserId), ct);
        return Ok(result);
    }

    // POST api/favoritos/{publicId}
    [HttpPost("{publicId:guid}")]
    public async Task<IActionResult> Agregar(Guid publicId, CancellationToken ct) {
        await _repo.AgregarAsync(UserId, publicId, ct);
        return NoContent();
    }

    // DELETE api/favoritos/{publicId}
    [HttpDelete("{publicId:guid}")]
    public async Task<IActionResult> Eliminar(Guid publicId, CancellationToken ct) {
        await _repo.EliminarAsync(UserId, publicId, ct);
        return NoContent();
    }
}
