using Application.CQRS.Commands.Auth;
using Application.CQRS.Core;
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
}
