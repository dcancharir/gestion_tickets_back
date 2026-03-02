using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class EstadoIncidenciaRepository : IEstadoIncidenciaRepository {
    private readonly ApplicationDbContext _db;
    public EstadoIncidenciaRepository(ApplicationDbContext db) => _db = db;

    public async Task<IEnumerable<EstadoIncidencia>> ObtenerTodosAsync(CancellationToken ct = default) =>
        await _db.EstadosIncidencia.AsNoTracking().OrderBy(e => e.EstadoId).ToListAsync(ct);

    public async Task<EstadoIncidencia?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        await _db.EstadosIncidencia.AsNoTracking().FirstOrDefaultAsync(e => e.EstadoId == id, ct);

    public async Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null, CancellationToken ct = default) =>
        await _db.EstadosIncidencia.AnyAsync(e => e.Nombre == nombre && (excluirId == null || e.EstadoId != excluirId), ct);

    public async Task<EstadoIncidencia> CrearAsync(EstadoIncidencia estado, CancellationToken ct = default) {
        _db.EstadosIncidencia.Add(estado);
        await _db.SaveChangesAsync(ct);
        return estado;
    }

    public async Task<EstadoIncidencia> ActualizarAsync(EstadoIncidencia estado, CancellationToken ct = default) {
        _db.EstadosIncidencia.Update(estado);
        await _db.SaveChangesAsync(ct);
        return estado;
    }

    public async Task EliminarAsync(int id, CancellationToken ct = default) {
        var estado = await _db.EstadosIncidencia.FindAsync(new object[] { id }, ct)
            ?? throw new KeyNotFoundException($"Estado con Id {id} no encontrado.");
        _db.EstadosIncidencia.Remove(estado);
        await _db.SaveChangesAsync(ct);
    }
}
