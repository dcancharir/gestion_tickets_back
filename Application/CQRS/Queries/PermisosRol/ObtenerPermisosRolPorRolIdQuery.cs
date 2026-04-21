using Application.CQRS.Core;
using Application.CQRS.Queries.Permisos;
using Application.DTOS.PermisoRol;
using Application.DTOS.Permisos;
using Application.Ports.Driven;

namespace Application.CQRS.Queries.PermisosRol;

public record ObtenerPermisosRolPorRolIdQuery(int rolId) : IQuery<IEnumerable<PermisoRolDto>>;

public class ObtenerPermisosRolPorRolIdQueryHandler : IQueryHandler<ObtenerPermisosRolPorRolIdQuery, IEnumerable<PermisoRolDto>> {
    private readonly IPermisoRolRepository _repo;
    public ObtenerPermisosRolPorRolIdQueryHandler(IPermisoRolRepository repo) => _repo = repo;

    public async Task<IEnumerable<PermisoRolDto>> HandleAsync(ObtenerPermisosRolPorRolIdQuery q, CancellationToken ct = default) {
        var permisoRols = await _repo.ObtenerTodosPorRolIdAsync(q.rolId,ct);

        return permisoRols.Select(c => new PermisoRolDto(c.PermisoRolId, c.PermisoId, c.RolId));
    }
}