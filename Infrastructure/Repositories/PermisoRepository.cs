using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PermisoRepository : IPermisoRepository
{
    private readonly ApplicationDbContext _db;

    public PermisoRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Permiso>> ObtenerTodosAsync(CancellationToken ct = default) =>
        await _db.Permisos.AsNoTracking()
            .ToListAsync(ct);

    public async Task<Permiso> CrearAsync(Permiso permiso, CancellationToken ct = default)
    {
        _db.Permisos.Add(permiso);
        await _db.SaveChangesAsync(ct);
        return permiso;
    }

    public async Task<bool> CrearRangoAsync(List<Permiso> permisos, CancellationToken ct = default)
    {
        await _db.Permisos.AddRangeAsync(permisos, ct);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken ct = default)
    {
        var permiso = await _db.Permisos.FindAsync(new object[]{id},ct)
            ?? throw new KeyNotFoundException($"Permiso con Id {id} no existe");
        _db.Permisos.Remove(permiso);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> EliminarRangoAsync(List<int> ids, CancellationToken ct = default)
    {
        var rows = await _db.Permisos
            .Where(x => ids.Contains(x.PermisoId))
            .ExecuteDeleteAsync(ct);
        return rows > 0;
    }
}