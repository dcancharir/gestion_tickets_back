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
    
    public async Task<PermisoRol> CrearAsync(PermisoRol permisoRol, CancellationToken ct = default)
    {
        _db.PermisosRol.Add(permisoRol);
        await _db.SaveChangesAsync(ct);
        return permisoRol;
    }

    public async Task<bool> CrearRangoAsync(List<PermisoRol> permisosRol, CancellationToken ct = default)
    {
        await _db.PermisosRol.AddRangeAsync(permisosRol, ct);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken ct = default)
    {
        var permisoRol = await _db.PermisosRol.FindAsync(new object[]{id},ct)
                      ?? throw new KeyNotFoundException($"Permiso con Id {id} no existe");
        _db.PermisosRol.Remove(permisoRol);
        await _db.SaveChangesAsync(ct);
        return true;
    }
    public async Task<bool> EliminarRangoAsync(List<int> ids, CancellationToken ct = default)
    {
        var rows = await _db.PermisosRol
            .Where(x => ids.Contains(x.PermisoRolId))
            .ExecuteDeleteAsync(ct);
        return rows > 0;
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

    public async Task<PermisoRol?> ObtenerPorPermisoYRol(int PermisoId, int RolId) {
        return await _db.PermisosRol.FirstOrDefaultAsync(x => x.PermisoId == PermisoId && x.RolId == RolId);
    }
}