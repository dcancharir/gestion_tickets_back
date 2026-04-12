using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class IncidenciaAdjuntoRepository : IIncidenciaAdjuntoRepository {
    private readonly ApplicationDbContext _db;
    public IncidenciaAdjuntoRepository(ApplicationDbContext db) {
        _db = db;
    }
    public async Task<IncidenciaAdjunto> CrearAsync(IncidenciaAdjunto item, CancellationToken ct) { 
        await _db.IncidenciaAdjuntos.AddAsync(item,ct);
        await _db.SaveChangesAsync(ct);
        return item;
    }

    public async Task<bool> CrearRangeAsync(List<IncidenciaAdjunto> items, CancellationToken ct) {
        await _db.IncidenciaAdjuntos.AddRangeAsync(items, ct);
        var result = await _db.SaveChangesAsync(ct);
        return result > 0;
    }

    public async Task EliminarAsync(int incidenciaAdjuntoId, CancellationToken ct) {
        var adjunto = await _db.IncidenciaAdjuntos.FindAsync(new object[] { incidenciaAdjuntoId }, ct)
          ?? throw new KeyNotFoundException($"Adjunto con Id {incidenciaAdjuntoId} no encontrada.");
        _db.IncidenciaAdjuntos.Remove(adjunto);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<IncidenciaAdjunto>> ObtenerPorIncidenciaAsync(int incidenciaId, CancellationToken ct) =>
        await _db.IncidenciaAdjuntos.AsNoTracking().Where(x => x.IncidenciaId == incidenciaId).ToListAsync(ct);
}
