using Application.CQRS.Core;
using Application.CQRS.Queries.Usuarios;
using Application.DTOS.Usuarios;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;

namespace Application.CQRS.Commands.Usuarios;

// ── Command ───────────────────────────────────────────────────────────────────
// Solo permite al propio usuario editar nombre, apellidos y email.
// No puede cambiarse RolId ni Activo (eso es admin).

public record ActualizarPerfilCommand(
    Guid   PublicId,
    string Nombre,
    string Apellidos,
    string Email
) : ICommand<UsuarioDto>;

// ── DTO de entrada ────────────────────────────────────────────────────────────

public record ActualizarPerfilDto(
    string Nombre,
    string Apellidos,
    string Email
);

// ── Handler ───────────────────────────────────────────────────────────────────

public class ActualizarPerfilHandler : ICommandHandler<ActualizarPerfilCommand, UsuarioDto> {
    private readonly IUsuarioRepository _repo;

    public ActualizarPerfilHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<UsuarioDto> HandleAsync(
        ActualizarPerfilCommand command,
        CancellationToken ct = default) {

        var usuario = await _repo.ObtenerPorPublicIdAsync(command.PublicId, ct)
            ?? throw new NotFoundException(nameof(Usuario), command.PublicId);

        usuario.Nombre    = command.Nombre.Trim();
        usuario.Apellidos = command.Apellidos.Trim();
        usuario.Email     = command.Email.Trim().ToLowerInvariant();

        var actualizado = await _repo.ActualizarAsync(usuario, ct);
        var conRol      = await _repo.ObtenerPorIdAsync(actualizado.UsuarioId, ct);

        return ObtenerUsuarioPorPublicIdHandler.ToDto(conRol!);
    }
}
