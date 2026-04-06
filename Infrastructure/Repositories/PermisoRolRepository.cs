using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PermisoRolRepository : IPermisoRolRepository
{
    private readonly ApplicationDbContext _db;

    public PermisoRolRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<IEnumerable<PermisoRol>> ObtenerTodosPorRolIdAsync(int rolId, CancellationToken ct = default) =>
    await _db.PermisosRol
        .AsNoTracking()
        .Include(p=>p.Permiso)
        .Where(x=>x.RolId == rolId)
        .ToListAsync(ct);

    public async Task<bool> VerificarSiRolTienePermisoDeVistaAsync(int rolId, string uri, CancellationToken ct = default) =>
    await _db.PermisosRol
    .AnyAsync(x=>
        x.RolId == rolId && 
        x.Permiso.Nombre.Equals(uri,StringComparison.OrdinalIgnoreCase) &&
        x.Permiso.Tipo.Equals("vista",StringComparison.OrdinalIgnoreCase)
        , ct);

    public async Task<bool> VerificarSiRolTienePermisoDeControladorAsync(int rolId, string actionName, string controllerName,
        CancellationToken ct = default) =>
        await _db.PermisosRol
            .AnyAsync(x => 
                x.RolId == rolId &&
                x.Permiso.Nombre.Equals(actionName,StringComparison.OrdinalIgnoreCase) &&
                x.Permiso.Tipo.Equals("permiso",StringComparison.OrdinalIgnoreCase) &&
                x.Permiso.Controlador.Equals(controllerName,StringComparison.OrdinalIgnoreCase)
                ,ct);
}