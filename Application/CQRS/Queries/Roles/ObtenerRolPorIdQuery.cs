using Application.CQRS.Core;
using Application.DTOS.Roles;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Roles;

public record ObtenerRolPorIdQuery(int RolId) : IQuery<RolDto>;

public class ObtenerRolPorIdHandler : IQueryHandler<ObtenerRolPorIdQuery, RolDto> {
    private readonly IRolRepository _repo;
    public ObtenerRolPorIdHandler(IRolRepository repo) => _repo = repo;

    public async Task<RolDto> HandleAsync(ObtenerRolPorIdQuery q, CancellationToken ct = default) {
        var rol = await _repo.ObtenerPorIdAsync(q.RolId, ct)
            ?? throw new NotFoundException(nameof(Rol), q.RolId);
        return new RolDto(rol.RolId, rol.Nombre, rol.Descripcion);
    }
}
