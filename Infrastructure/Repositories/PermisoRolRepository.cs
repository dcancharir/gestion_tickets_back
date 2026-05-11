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

    public async Task<bool> VerificarSiRolTienePermisoDeVistaAsync(int rolId, string uri, CancellationToken ct = default)
    {
        uri = uri.ToLower();

        // Solo restringir si la vista está registrada Y al menos un rol la tiene asignada.
        // Si nadie la tiene asignada aún, se considera no configurada → se deja pasar.
        var asignadaAlgunRol = await _db.PermisosRol.AnyAsync(x =>
            x.Permiso.Nombre.ToLower() == uri &&
            x.Permiso.Tipo.ToLower()   == "vista",
            ct);

        if (!asignadaAlgunRol)
            return true;

        // La vista ya fue configurada → verificar si este rol la tiene
        return await _db.PermisosRol.AnyAsync(x =>
            x.RolId                    == rolId &&
            x.Permiso.Nombre.ToLower() == uri &&
            x.Permiso.Tipo.ToLower()   == "vista",
            ct);
    }

    public async Task<bool> VerificarSiRolTienePermisoDeControladorAsync(
       int rolId,
       string actionName,
       string controllerName,
       CancellationToken ct = default)
    {
        controllerName = controllerName.Replace("Controller", "").ToLower();
        actionName     = actionName.ToLower();

        // Solo restringir si el permiso está registrado Y al menos un rol lo tiene asignado.
        // Si nadie lo tiene asignado aún, se considera no configurado → se deja pasar.
        var asignadoAlgunRol = await _db.PermisosRol.AnyAsync(x =>
            x.Permiso.Nombre.ToLower()                           == actionName &&
            x.Permiso.Controlador.Replace("Controller", "").ToLower() == controllerName &&
            x.Permiso.Tipo.ToLower()                             == "permiso",
            ct);

        if (!asignadoAlgunRol)
            return true;

        // El permiso ya fue configurado para algún rol → verificar si este rol lo tiene
        return await _db.PermisosRol.AnyAsync(x =>
            x.RolId                                              == rolId &&
            x.Permiso.Nombre.ToLower()                           == actionName &&
            x.Permiso.Controlador.Replace("Controller", "").ToLower() == controllerName &&
            x.Permiso.Tipo.ToLower()                             == "permiso",
            ct);
    }

    public async Task<PermisoRol?> ObtenerPorPermisoYRol(int PermisoId, int RolId) {
        return await _db.PermisosRol.FirstOrDefaultAsync(x => x.PermisoId == PermisoId && x.RolId == RolId);
    }

    public async Task<bool> EliminarPorPermisoIdsAsync(List<int> permisoIds, CancellationToken ct = default)
    {
        if (permisoIds.Count == 0) return true;
        var rows = await _db.PermisosRol
            .Where(x => permisoIds.Contains(x.PermisoId))
            .ExecuteDeleteAsync(ct);
        return rows >= 0;
    }
}