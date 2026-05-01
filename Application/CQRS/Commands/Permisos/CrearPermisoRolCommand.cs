using Application.CQRS.Core;
using Application.DTOS.PermisoRol;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Application.CQRS.Commands.Permisos;

public record CrearPermisoRolCommand (int PermisoId, int RolId) : ICommand<PermisoRolDto>;
//Handler
public class CrearPermisoRolHandler: ICommandHandler<CrearPermisoRolCommand, PermisoRolDto> {
    private readonly IPermisoRolRepository _repo;
    public CrearPermisoRolHandler(IPermisoRolRepository repo) {
        _repo = repo;
    }
    public async Task<PermisoRolDto> HandleAsync(
        CrearPermisoRolCommand command,
        CancellationToken ct = default
        ) {
        var permisoRol = new PermisoRol() { 
            PermisoId = command.PermisoId,
            RolId = command.RolId
        };
        var creado = await _repo.CrearAsync(permisoRol,ct);

        return new PermisoRolDto(creado.PermisoRolId, permisoRol.PermisoRolId, permisoRol.RolId);
    }
}