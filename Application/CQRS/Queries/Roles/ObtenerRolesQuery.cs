using Application.CQRS.Core;
using Application.DTOS.Roles;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Roles;

public record ObtenerRolesQuery() : IQuery<IEnumerable<RolDto>>;

public class ObtenerRolesHandler : IQueryHandler<ObtenerRolesQuery, IEnumerable<RolDto>> {
    private readonly IRolRepository _repo;
    public ObtenerRolesHandler(IRolRepository repo) => _repo = repo;

    public async Task<IEnumerable<RolDto>> HandleAsync(ObtenerRolesQuery q, CancellationToken ct = default) {
        var roles = await _repo.ObtenerTodosAsync(ct);
        return roles.Select(r => new RolDto(r.RolId, r.Nombre, r.Descripcion));
    }
}
