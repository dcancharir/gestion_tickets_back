using Application.CQRS.Core;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Roles;

public record EliminarRolCommand(int RolId) : ICommand;

public class EliminarRolHandler : ICommandHandler<EliminarRolCommand> {
    private readonly IRolRepository _repo;
    public EliminarRolHandler(IRolRepository repo) => _repo = repo;

    public async Task<Unit> HandleAsync(EliminarRolCommand cmd, CancellationToken ct = default) {
        _ = await _repo.ObtenerPorIdAsync(cmd.RolId, ct)
            ?? throw new NotFoundException(nameof(Rol), cmd.RolId);
        await _repo.EliminarAsync(cmd.RolId, ct);
        return Unit.Value;
    }
}
