using Application.CQRS.Core;
using Application.Exceptions;
using Application.Ports.Driven;

namespace Application.CQRS.Commands.Auth;

// ── Command ───────────────────────────────────────────────────────────────────
// El usuario autenticado cambia su propia contraseña.
// Requiere la contraseña actual para confirmar identidad.

public record CambiarPasswordCommand(
    Guid   UsuarioPublicId,
    string PasswordActual,
    string NuevoPassword
) : ICommand<bool>;

// ── DTO de entrada ────────────────────────────────────────────────────────────

public record CambiarPasswordDto(
    string PasswordActual,
    string NuevoPassword,
    string ConfirmarPassword
);

// ── Handler ───────────────────────────────────────────────────────────────────

public class CambiarPasswordHandler : ICommandHandler<CambiarPasswordCommand, bool> {
    private readonly IUsuarioRepository _repo;

    public CambiarPasswordHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<bool> HandleAsync(
        CambiarPasswordCommand command,
        CancellationToken ct = default) {

        // 1. Obtener usuario con su hash actual
        var usuario = await _repo.ObtenerPorPublicIdAsync(command.UsuarioPublicId, ct)
            ?? throw new NotFoundException("Usuario", command.UsuarioPublicId);

        // 2. Verificar contraseña actual
        if (!BCrypt.Net.BCrypt.Verify(command.PasswordActual, usuario.PasswordHash))
            throw new ConflictException("La contraseña actual es incorrecta.");

        // 3. No permitir que la nueva sea igual a la actual
        if (BCrypt.Net.BCrypt.Verify(command.NuevoPassword, usuario.PasswordHash))
            throw new ConflictException("La nueva contraseña no puede ser igual a la actual.");

        // 4. Validar longitud mínima
        if (command.NuevoPassword.Length < 8)
            throw new ConflictException("La contraseña debe tener al menos 8 caracteres.");

        // 5. Hashear y guardar
        var nuevoHash = BCrypt.Net.BCrypt.HashPassword(command.NuevoPassword, workFactor: 11);
        await _repo.ActualizarPasswordAsync(usuario.UsuarioId, nuevoHash, ct);

        return true;
    }
}
