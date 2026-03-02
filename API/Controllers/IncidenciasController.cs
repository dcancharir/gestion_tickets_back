using Application.CQRS.Commands.Incidencias;
using Application.CQRS.Core;
using Application.CQRS.Queries.Incidencias;
using Application.DTOS.Incidencias;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidenciasController : ControllerBase {
    private readonly IDispatcher _dispatcher;

    public IncidenciasController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    // ── Helpers JWT (temporales hasta implementar autenticación) ──────────────
    // Cuando implementes JWT estos se reemplazan por:
    //   int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)
    //   int.Parse(User.FindFirstValue("RolId")!)
    private int GetUsuarioId() =>
        int.Parse(Request.Headers["X-Usuario-Id"].FirstOrDefault() ?? "0");

    private int GetRolId() =>
        int.Parse(Request.Headers["X-Rol-Id"].FirstOrDefault() ?? "3");

    // ── Queries ───────────────────────────────────────────────────────────────

    // GET api/incidencias
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IncidenciaListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(new ObtenerIncidenciasQuery(), ct);
        return Ok(result);
    }

    // GET api/incidencias/mis-tickets
    [HttpGet("mis-tickets")]
    [ProducesResponseType(typeof(IEnumerable<IncidenciaListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMisTickets(CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(
            new ObtenerMisIncidenciasQuery(GetUsuarioId()), ct);
        return Ok(result);
    }

    // GET api/incidencias/mis-asignaciones
    [HttpGet("mis-asignaciones")]
    [ProducesResponseType(typeof(IEnumerable<IncidenciaListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMisAsignaciones(CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(
            new ObtenerIncidenciasPorTecnicoQuery(GetUsuarioId()), ct);
        return Ok(result);
    }

    // GET api/incidencias/{publicId}
    [HttpGet("{publicId:guid}")]
    [ProducesResponseType(typeof(IncidenciaDetalleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid publicId, CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(
            new ObtenerIncidenciaPorPublicIdQuery(publicId), ct);
        return Ok(result);
    }

    // ── Commands ──────────────────────────────────────────────────────────────

    // POST api/incidencias — cualquier rol puede crear
    [HttpPost]
    [ProducesResponseType(typeof(IncidenciaListItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] RegistrarIncidenciaDto dto, CancellationToken ct) {
        var command = new RegistrarIncidenciaCommand(
            dto.Titulo,
            dto.Descripcion,
            dto.CategoriaId,
            dto.CanalReporte,
            dto.Impacto,
            dto.Urgencia,
            dto.PrioridadId,
            GetUsuarioId()
        );

        var result = await _dispatcher.SendAsync(command, ct);
        return CreatedAtAction(nameof(GetById), new { publicId = result.PublicId }, result);
    }

    // PATCH api/incidencias/{publicId}/asignar
    // Admin asigna a cualquier técnico / Técnico se auto-asigna
    [HttpPatch("{publicId:guid}/asignar")]
    [ProducesResponseType(typeof(IncidenciaListItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Asignar(
        Guid publicId,
        [FromBody] AsignarIncidenciaDto dto,
        CancellationToken ct) {
        var command = new AsignarIncidenciaCommand(
            publicId,
            dto.TecnicoPublicId,
            GetUsuarioId(),
            GetRolId()
        );

        var result = await _dispatcher.SendAsync(command, ct);
        return Ok(result);
    }

    // PATCH api/incidencias/{publicId}/estado
    [HttpPatch("{publicId:guid}/estado")]
    [ProducesResponseType(typeof(IncidenciaListItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CambiarEstado(
        Guid publicId,
        [FromBody] CambiarEstadoDto dto,
        CancellationToken ct) {
        var command = new CambiarEstadoCommand(
            publicId, dto.NuevoEstadoId, dto.Detalle, GetUsuarioId());

        var result = await _dispatcher.SendAsync(command, ct);
        return Ok(result);
    }

    // PATCH api/incidencias/{publicId}/resolver
    [HttpPatch("{publicId:guid}/resolver")]
    [ProducesResponseType(typeof(IncidenciaListItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Resolver(
        Guid publicId,
        [FromBody] ResolverIncidenciaDto dto,
        CancellationToken ct) {
        var command = new ResolverIncidenciaCommand(
            publicId,
            dto.SolucionAplicada,
            dto.ResueltoEnPrimerContacto,
            GetUsuarioId()
        );

        var result = await _dispatcher.SendAsync(command, ct);
        return Ok(result);
    }

    // PATCH api/incidencias/{publicId}/cerrar
    // Admin, Técnico (si lo tiene asignado) y Solicitante (si es suyo) pueden cerrar
    [HttpPatch("{publicId:guid}/cerrar")]
    [ProducesResponseType(typeof(IncidenciaListItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cerrar(
        Guid publicId,
        [FromBody] CerrarIncidenciaDto dto,
        CancellationToken ct) {
        var command = new CerrarIncidenciaCommand(
            publicId,
            GetUsuarioId(),
            GetRolId(),
            dto.Comentario
        );

        var result = await _dispatcher.SendAsync(command, ct);
        return Ok(result);
    }

    // PATCH api/incidencias/{publicId}/escalar
    [HttpPatch("{publicId:guid}/escalar")]
    [ProducesResponseType(typeof(IncidenciaListItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Escalar(
        Guid publicId,
        [FromBody] EscalarIncidenciaDto dto,
        CancellationToken ct) {
        var command = new EscalarIncidenciaCommand(
            publicId, dto.TecnicoPublicId, dto.Motivo, GetUsuarioId());

        var result = await _dispatcher.SendAsync(command, ct);
        return Ok(result);
    }

    // PATCH api/incidencias/{publicId}/reabrir
    [HttpPatch("{publicId:guid}/reabrir")]
    [ProducesResponseType(typeof(IncidenciaListItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reabrir(
        Guid publicId,
        [FromBody] ReabrirIncidenciaDto dto,
        CancellationToken ct) {
        var command = new ReabrirIncidenciaCommand(
            publicId, dto.Motivo, GetUsuarioId());

        var result = await _dispatcher.SendAsync(command, ct);
        return Ok(result);
    }

    // POST api/incidencias/{publicId}/comentarios
    [HttpPost("{publicId:guid}/comentarios")]
    [ProducesResponseType(typeof(ComentarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AgregarComentario(
        Guid publicId,
        [FromBody] AgregarComentarioDto dto,
        CancellationToken ct) {
        var command = new AgregarComentarioCommand(
            publicId, dto.Mensaje, dto.EsInterno, GetUsuarioId());

        var result = await _dispatcher.SendAsync(command, ct);
        return Created(string.Empty, result);
    }
}