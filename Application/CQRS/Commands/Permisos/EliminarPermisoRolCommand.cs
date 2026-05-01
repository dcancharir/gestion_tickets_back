using Application.CQRS.Core;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Permisos;

public record EliminarPermisoRolCommand(int PermisoId, int RolId) : ICommand;
public class EliminarPermisoRolHandler : ICommandHandler<EliminarPermisoRolCommand> {
    private readonly IPermisoRolRepository _repo;

    public EliminarPermisoRolHandler(IPermisoRolRepository repo) {
        _repo = repo;
    }

    public async Task<Unit> HandleAsync(EliminarPermisoRolCommand command, CancellationToken cancellationToken = default) {
        var permisoRol = await _repo.ObtenerPorPermisoYRol(command.PermisoId, command.RolId);
        if(permisoRol == null) {
            throw new NotFoundException(nameof(PermisoRol), new { command.PermisoId , command.RolId });
        }
        await _repo.EliminarAsync(permisoRol.PermisoRolId, cancellationToken)    ;
        return Unit.Value;
    }
}