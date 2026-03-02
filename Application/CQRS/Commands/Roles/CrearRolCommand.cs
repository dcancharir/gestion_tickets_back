using Application.CQRS.Core;
using Application.DTOS.Roles;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Roles;

public record CrearRolCommand(string Nombre, string? Descripcion) : ICommand<RolDto>;

public class CrearRolHandler : ICommandHandler<CrearRolCommand, RolDto> {
    private readonly IRolRepository _repo;
    public CrearRolHandler(IRolRepository repo) => _repo = repo;

    public async Task<RolDto> HandleAsync(CrearRolCommand cmd, CancellationToken ct = default) {
        if(await _repo.ExisteNombreAsync(cmd.Nombre, ct: ct))
            throw new ConflictException($"El rol '{cmd.Nombre}' ya existe.");

        var rol = new Rol { Nombre = cmd.Nombre, Descripcion = cmd.Descripcion };
        var creado = await _repo.CrearAsync(rol, ct);
        return new RolDto(creado.RolId, creado.Nombre, creado.Descripcion);
    }
}
