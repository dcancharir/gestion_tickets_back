using Application.CQRS.Core;
using Application.DTOS.Auth;
using Application.Exceptions;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Auth;

public record LoginCommand(
    string UserName,
    string Password
) : ICommand<TokenResponseDto>;

// ── Handler ───────────────────────────────────────────────────────────────────

public class LoginHandler : ICommandHandler<LoginCommand, TokenResponseDto> {
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ITokenService _tokenService;

    public LoginHandler(IUsuarioRepository usuarioRepo, ITokenService tokenService) {
        _usuarioRepo = usuarioRepo;
        _tokenService = tokenService;
    }

    public async Task<TokenResponseDto> HandleAsync(
        LoginCommand command, CancellationToken ct = default) {
        // 1. Buscar usuario por email
        var usuario = await _usuarioRepo.ObtenerPorUserNameAsync(command.UserName, ct)
            ?? throw new UnauthorizedException("Credenciales inválidas.");

        // 2. Verificar que la cuenta esté activa
        if(!usuario.Activo)
            throw new UnauthorizedException("La cuenta está desactivada.");

        // 3. Verificar contraseña con BCrypt
        if(!BCrypt.Net.BCrypt.Verify(command.Password, usuario.PasswordHash))
            throw new UnauthorizedException("Credenciales inválidas.");

        // 4. Generar token y retornar respuesta
        var token = _tokenService.GenerarToken(usuario);
        var expiracion = DateTime.UtcNow.AddHours(8);

        return new TokenResponseDto(
            token,
            expiracion,
            usuario.PublicId,
            usuario.Nombre,
            usuario.Apellidos,
            usuario.Email,
            usuario.Rol.Nombre,
            usuario.UserName
        );
    }
}
