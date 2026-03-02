using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class AcuerdoNivelServicioRepository : IAcuerdoNivelServicioRepository {
    private readonly ApplicationDbContext _db;
    public AcuerdoNivelServicioRepository(ApplicationDbContext db) => _db = db;

    public async Task<IEnumerable<AcuerdoNivelServicio>> ObtenerTodosAsync(CancellationToken ct = default) =>
        await _db.AcuerdosNivelServicio
            .AsNoTracking()
            .Include(s => s.Categoria)
            .Include(s => s.NivelPrioridad)
            .OrderBy(s => s.Categoria.Nombre)
            .ThenBy(s => s.NivelPrioridad.Nivel)
            .ToListAsync(ct);

    public async Task<AcuerdoNivelServicio?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        await _db.AcuerdosNivelServicio
            .AsNoTracking()
            .Include(s => s.Categoria)
            .Include(s => s.NivelPrioridad)
            .FirstOrDefaultAsync(s => s.SlaId == id, ct);

    public async Task<AcuerdoNivelServicio?> ObtenerPorCategoriaYPrioridadAsync(
        int categoriaId, int prioridadId, CancellationToken ct = default) =>
        await _db.AcuerdosNivelServicio
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.CategoriaId == categoriaId && s.PrioridadId == prioridadId, ct);

    public async Task<bool> ExisteCombinacionAsync(int categoriaId, int prioridadId, int? excluirId = null, CancellationToken ct = default) =>
        await _db.AcuerdosNivelServicio.AnyAsync(
            s => s.CategoriaId == categoriaId
              && s.PrioridadId == prioridadId
              && (excluirId == null || s.SlaId != excluirId), ct);

    public async Task<AcuerdoNivelServicio> CrearAsync(AcuerdoNivelServicio sla, CancellationToken ct = default) {
        _db.AcuerdosNivelServicio.Add(sla);
        await _db.SaveChangesAsync(ct);
        return sla;
    }

    public async Task<AcuerdoNivelServicio> ActualizarAsync(AcuerdoNivelServicio sla, CancellationToken ct = default) {
        _db.AcuerdosNivelServicio.Update(sla);
        await _db.SaveChangesAsync(ct);
        return sla;
    }

    public async Task EliminarAsync(int id, CancellationToken ct = default) {
        var sla = await _db.AcuerdosNivelServicio.FindAsync(new object[] { id }, ct)
            ?? throw new KeyNotFoundException($"SLA con Id {id} no encontrado.");
        _db.AcuerdosNivelServicio.Remove(sla);
        await _db.SaveChangesAsync(ct);
    }
}
