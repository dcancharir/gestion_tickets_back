using Application.CQRS.Core;
using Application.DTOS.Roles;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Roles;

public record ActualizarRolCommand(int RolId, string Nombre, string? Descripcion) : ICommand<RolDto>;

public class ActualizarRolHandler : ICommandHandler<ActualizarRolCommand, RolDto> {
    private readonly IRolRepository _repo;
    public ActualizarRolHandler(IRolRepository repo) => _repo = repo;

    public async Task<RolDto> HandleAsync(ActualizarRolCommand cmd, CancellationToken ct = default) {
        var rol = await _repo.ObtenerPorIdAsync(cmd.RolId, ct)
            ?? throw new NotFoundException(nameof(Rol), cmd.RolId);

        if(await _repo.ExisteNombreAsync(cmd.Nombre, excluirId: cmd.RolId, ct: ct))
            throw new ConflictException($"El nombre '{cmd.Nombre}' ya está en uso.");

        rol.Nombre = cmd.Nombre;
        rol.Descripcion = cmd.Descripcion;

        var actualizado = await _repo.ActualizarAsync(rol, ct);
        return new RolDto(actualizado.RolId, actualizado.Nombre, actualizado.Descripcion);
    }
}
