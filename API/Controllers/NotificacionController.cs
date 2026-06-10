using API.Extensions;
using Application.DTOS.Notificaciones;
using Application.Ports.Driven;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificacionController : ControllerBase {
    private readonly INotificacionRepository _repo;
    private int GetUsuarioId() => User.GetUsuarioId();

    public NotificacionController(INotificacionRepository repo) => _repo = repo;

    /// <summary>
    /// Devuelve las notificaciones no leídas del usuario autenticado.
    /// Se usa al iniciar sesión para mostrar las pendientes.
    /// </summary>
    [HttpGet("mis-notificaciones")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<NotificacionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMisNotificaciones(CancellationToken ct) {
        if (User.Identity?.IsAuthenticated != true)
            return Ok(Array.Empty<NotificacionDto>());

        var lista = await _repo.ObtenerNoLeidasPorUsuarioAsync(GetUsuarioId(), ct);

        return Ok(lista.Select(n => new NotificacionDto(
            n.NotificacionId,
            n.Tipo,
            n.TicketPublicId,
            n.NumeroTicket,
            n.Titulo,
            n.Mensaje,
            n.Leida,
            n.FechaCreacion)));
    }

    /// <summary>Marca una notificación específica como leída.</summary>
    [HttpPut("{id:int}/leer")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> MarcarLeida(int id, CancellationToken ct) {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        await _repo.MarcarComoLeidaAsync(id, GetUsuarioId(), ct);
        return NoContent();
    }

    /// <summary>Marca todas las notificaciones del usuario como leídas.</summary>
    [HttpPut("leer-todas")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> MarcarTodasLeidas(CancellationToken ct) {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        await _repo.MarcarTodasLeidasAsync(GetUsuarioId(), ct);
        return NoContent();
    }
}
