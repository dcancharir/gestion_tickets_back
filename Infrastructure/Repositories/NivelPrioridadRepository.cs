using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class NivelPrioridadRepository : INivelPrioridadRepository {
    private readonly ApplicationDbContext _db;
    public NivelPrioridadRepository(ApplicationDbContext db) => _db = db;

    public async Task<IEnumerable<NivelPrioridad>> ObtenerTodosAsync(CancellationToken ct = default) =>
        await _db.NivelesPrioridad.AsNoTracking().OrderBy(n => n.Nivel).ToListAsync(ct);

    public async Task<NivelPrioridad?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        await _db.NivelesPrioridad.AsNoTracking().FirstOrDefaultAsync(n => n.PrioridadId == id, ct);

    public async Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null, CancellationToken ct = default) =>
        await _db.NivelesPrioridad.AnyAsync(n => n.Nombre == nombre && (excluirId == null || n.PrioridadId != excluirId), ct);

    public async Task<NivelPrioridad> CrearAsync(NivelPrioridad nivel, CancellationToken ct = default) {
        _db.NivelesPrioridad.Add(nivel);
        await _db.SaveChangesAsync(ct);
        return nivel;
    }

    public async Task<NivelPrioridad> ActualizarAsync(NivelPrioridad nivel, CancellationToken ct = default) {
        _db.NivelesPrioridad.Update(nivel);
        await _db.SaveChangesAsync(ct);
        return nivel;
    }

    public async Task EliminarAsync(int id, CancellationToken ct = default) {
        var nivel = await _db.NivelesPrioridad.FindAsync(new object[] { id }, ct)
            ?? throw new KeyNotFoundException($"Nivel de prioridad con Id {id} no encontrado.");
        _db.NivelesPrioridad.Remove(nivel);
        await _db.SaveChangesAsync(ct);
    }
}
