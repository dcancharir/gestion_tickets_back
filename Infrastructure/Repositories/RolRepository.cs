using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class RolRepository : IRolRepository {
    private readonly ApplicationDbContext _db;
    public RolRepository(ApplicationDbContext db) => _db = db;

    public async Task<IEnumerable<Rol>> ObtenerTodosAsync(CancellationToken ct = default) =>
        await _db.Roles.AsNoTracking().OrderBy(r => r.RolId).ToListAsync(ct);

    public async Task<Rol?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        await _db.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RolId == id, ct);

    public async Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null, CancellationToken ct = default) =>
        await _db.Roles.AnyAsync(r => r.Nombre == nombre && (excluirId == null || r.RolId != excluirId), ct);

    public async Task<Rol> CrearAsync(Rol rol, CancellationToken ct = default) {
        _db.Roles.Add(rol);
        await _db.SaveChangesAsync(ct);
        return rol;
    }

    public async Task<Rol> ActualizarAsync(Rol rol, CancellationToken ct = default) {
        _db.Roles.Update(rol);
        await _db.SaveChangesAsync(ct);
        return rol;
    }

    public async Task EliminarAsync(int id, CancellationToken ct = default) {
        var rol = await _db.Roles.FindAsync(new object[] { id }, ct)
            ?? throw new KeyNotFoundException($"Rol con Id {id} no encontrado.");
        _db.Roles.Remove(rol);
        await _db.SaveChangesAsync(ct);
    }
}
