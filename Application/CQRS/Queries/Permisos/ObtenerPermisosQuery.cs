using Application.CQRS.Core;
using Application.DTOS.Permisos;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Permisos;

public record ObtenerPermisosQuery() : IQuery<IEnumerable<PermisoDto>>;

public class ObtenerPermisosHandler : IQueryHandler<ObtenerPermisosQuery, IEnumerable<PermisoDto>> {
    private readonly IPermisoRepository _repo;
    public ObtenerPermisosHandler(IPermisoRepository repo) => _repo = repo;

    public async Task<IEnumerable<PermisoDto>> HandleAsync(ObtenerPermisosQuery q, CancellationToken ct = default) {
        var Permisos = await _repo.ObtenerTodosAsync(ct);

        return Permisos.Select(c => new PermisoDto(c.PermisoId, c.Nombre, c.Tipo, c.Controlador));
    }
}
