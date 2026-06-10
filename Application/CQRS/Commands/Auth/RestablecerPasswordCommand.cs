using Application.CQRS.Core;
using Application.Exceptions;
using Application.Ports.Driven;

namespace Application.CQRS.Commands.Auth;

// ── Command ───────────────────────────────────────────────────────────────────
// Recibe el token de la URL y la nueva contraseña elegida por el usuario.
// Ruta pública — no requiere autenticación.

public record RestablecerPasswordCommand(
    string Token,
    string NuevoPassword
) : ICommand<bool>;

// ── Handler ───────────────────────────────────────────────────────────────────

public class RestablecerPasswordHandler
    : ICommandHandler<RestablecerPasswordCommand, bool> {

    private readonly IUsuarioRepository _repo;

    public RestablecerPasswordHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<bool> HandleAsync(
        RestablecerPasswordCommand command,
        CancellationToken ct = default) {

        // 1. Buscar usuario con ese token
        var usuario = await _repo.ObtenerPorTokenRecuperacionAsync(command.Token, ct)
            ?? throw new ConflictException("El enlace de recuperación no es válido o ya fue utilizado.");

        // 2. Verificar expiración
        if (usuario.TokenExpiracion is null || usuario.TokenExpiracion < DateTime.UtcNow)
            throw new ConflictException("El enlace de recuperación ha expirado. Solicita uno nuevo.");

        // 3. Validar longitud mínima de la nueva contraseña
        if (command.NuevoPassword.Length < 8)
            throw new ConflictException("La contraseña debe tener al menos 8 caracteres.");

        // 4. Hashear y guardar
        var nuevoHash = BCrypt.Net.BCrypt.HashPassword(command.NuevoPassword, workFactor: 11);
        await _repo.ActualizarPasswordAsync(usuario.UsuarioId, nuevoHash, ct);

        return true;
    }
}
