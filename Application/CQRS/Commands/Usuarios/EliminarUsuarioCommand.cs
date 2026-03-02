using Application.CQRS.Core;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Usuarios;

// ── Command ───────────────────────────────────────────────────────────────────

public record EliminarUsuarioCommand(Guid PublicId) : ICommand;  // ← Guid, no int

// ── Handler ───────────────────────────────────────────────────────────────────

public class EliminarUsuarioHandler : ICommandHandler<EliminarUsuarioCommand> {
    private readonly IUsuarioRepository _repo;

    public EliminarUsuarioHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<Unit> HandleAsync(
        EliminarUsuarioCommand command,
        CancellationToken ct = default) {
        // Resuelve Guid → int internamente, el frontend nunca ve el int
        var usuario = await _repo.ObtenerPorPublicIdAsync(command.PublicId, ct)
            ?? throw new NotFoundException(nameof(Usuario), command.PublicId);

        await _repo.EliminarAsync(usuario.UsuarioId, ct);

        return Unit.Value;
    }
}
