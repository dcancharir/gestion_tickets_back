using Application.CQRS.Commands.Auth;
using Application.CQRS.Core;
using Application.CQRS.Queries.Auth;
using Application.DTOS.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly IDispatcher _dispatcher;

    public AuthController(IDispatcher dispatcher) => _dispatcher = dispatcher;

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct) {
        var result = await _dispatcher.SendAsync(new LoginCommand(dto.UserName, dto.Password), ct);
        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Me() {
        return Ok(new {
            UsuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0"),
            PublicId = Guid.Parse(User.FindFirstValue("PublicId") ?? Guid.Empty.ToString()),
            Email = User.FindFirstValue(ClaimTypes.Email),
            Nombre = User.FindFirstValue(ClaimTypes.Name),
            RolId = int.Parse(User.FindFirstValue("RolId") ?? "0"),
            Rol = User.FindFirstValue(ClaimTypes.Role),
            UserName = User.FindFirstValue("UserName"),
        });
    }

    // POST api/auth/solicitar-recuperacion
    // El administrador solicita el restablecimiento de contraseña para un usuario.
    // Genera un token, lo persiste y envía el correo al usuario.
    [HttpPost("solicitar-recuperacion")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SolicitarRecuperacion(
        [FromBody] SolicitarRecuperacionDto dto,
        CancellationToken ct) {
        await _dispatcher.SendAsync(
            new SolicitarRecuperacionCommand(dto.UsuarioPublicId), ct);
        return Ok(new { mensaje = "Correo de recuperación enviado." });
    }

    // GET api/auth/validar-token/{token}
    // El frontend lo llama al cargar la página de restablecimiento.
    // Ruta pública — no requiere autenticación.
    [HttpGet("validar-token/{token}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(TokenValidoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidarToken(string token, CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(
            new ValidarTokenRecuperacionQuery(token), ct);
        return Ok(result);
    }

    // POST api/auth/restablecer-password
    // El usuario envía el token (de la URL) y su nueva contraseña.
    // Ruta pública — no requiere autenticación.
    [HttpPost("restablecer-password")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RestablecerPassword(
        [FromBody] RestablecerPasswordDto dto,
        CancellationToken ct) {

        if (dto.NuevoPassword != dto.ConfirmarPassword)
            return BadRequest(new { mensaje = "Las contraseñas no coinciden." });

        await _dispatcher.SendAsync(
            new RestablecerPasswordCommand(dto.Token, dto.NuevoPassword), ct);

        return Ok(new { mensaje = "Contraseña restablecida correctamente." });
    }
}

// ── DTOs de entrada ───────────────────────────────────────────────────────────
public record SolicitarRecuperacionDto(Guid UsuarioPublicId);
