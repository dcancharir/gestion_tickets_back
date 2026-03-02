using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class IncidenciaRepository : IIncidenciaRepository {
    private readonly ApplicationDbContext _db;

    public IncidenciaRepository(ApplicationDbContext db) => _db = db;

    // ── Consultas ─────────────────────────────────────────────────────────────

    public async Task<IEnumerable<Incidencia>> ObtenerTodasAsync(CancellationToken ct = default) =>
        await _db.Incidencias
            .AsNoTracking()
            .Include(i => i.Categoria)
            .Include(i => i.NivelPrioridad)
            .Include(i => i.EstadoIncidencia)
            .Include(i => i.Solicitante)
            .Include(i => i.TecnicoAsignado)
            .OrderByDescending(i => i.FechaRegistro)
            .ToListAsync(ct);

    public async Task<Incidencia?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        await _db.Incidencias
            .AsNoTracking()
            .Include(i => i.Categoria)
            .Include(i => i.NivelPrioridad)
            .Include(i => i.EstadoIncidencia)
            .Include(i => i.Solicitante)
            .Include(i => i.TecnicoAsignado)
            .Include(i => i.EscaladoA)
            .Include(i => i.CerradoPor)
            .FirstOrDefaultAsync(i => i.IncidenciaId == id, ct);

    public async Task<Incidencia?> ObtenerPorPublicIdAsync(Guid publicId, CancellationToken ct = default) =>
        await _db.Incidencias
            .AsNoTracking()
            .Include(i => i.Categoria)
            .Include(i => i.NivelPrioridad)
            .Include(i => i.EstadoIncidencia)
            .Include(i => i.Solicitante)
            .Include(i => i.TecnicoAsignado)
            .Include(i => i.EscaladoA)
            .Include(i => i.CerradoPor)
            .FirstOrDefaultAsync(i => i.PublicId == publicId, ct);

    public async Task<IEnumerable<Incidencia>> ObtenerPorSolicitanteAsync(
        int solicitanteId, CancellationToken ct = default) =>
        await _db.Incidencias
            .AsNoTracking()
            .Include(i => i.Categoria)
            .Include(i => i.NivelPrioridad)
            .Include(i => i.EstadoIncidencia)
            .Where(i => i.SolicitanteId == solicitanteId)
            .OrderByDescending(i => i.FechaRegistro)
            .ToListAsync(ct);

    public async Task<IEnumerable<Incidencia>> ObtenerPorTecnicoAsync(
        int tecnicoId, CancellationToken ct = default) =>
        await _db.Incidencias
            .AsNoTracking()
            .Include(i => i.Categoria)
            .Include(i => i.NivelPrioridad)
            .Include(i => i.EstadoIncidencia)
            .Include(i => i.Solicitante)
            .Where(i => i.TecnicoAsignadoId == tecnicoId)
            .OrderByDescending(i => i.FechaRegistro)
            .ToListAsync(ct);

    public async Task<int> ContarPorEstadoAsync(int estadoId, CancellationToken ct = default) =>
        await _db.Incidencias.CountAsync(i => i.EstadoId == estadoId, ct);

    // ── Persistencia ──────────────────────────────────────────────────────────

    public async Task<Incidencia> CrearAsync(Incidencia incidencia, CancellationToken ct = default) {
        _db.Incidencias.Add(incidencia);
        await _db.SaveChangesAsync(ct);
        return incidencia;
    }

    public async Task<Incidencia> ActualizarAsync(Incidencia incidencia, CancellationToken ct = default) {
        _db.Incidencias.Update(incidencia);
        await _db.SaveChangesAsync(ct);
        return incidencia;
    }

    // ── Historial ─────────────────────────────────────────────────────────────

    public async Task AgregarHistorialAsync(
        HistorialIncidencia historial, CancellationToken ct = default) {
        _db.HistorialIncidencias.Add(historial);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<HistorialIncidencia>> ObtenerHistorialAsync(
        int incidenciaId, CancellationToken ct = default) =>
        await _db.HistorialIncidencias
            .AsNoTracking()
            .Include(h => h.Usuario)
            .Where(h => h.IncidenciaId == incidenciaId)
            .OrderBy(h => h.FechaAccion)
            .ToListAsync(ct);

    // ── Comentarios ───────────────────────────────────────────────────────────

    public async Task AgregarComentarioAsync(
        ComentarioIncidencia comentario, CancellationToken ct = default) {
        _db.ComentariosIncidencia.Add(comentario);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<ComentarioIncidencia>> ObtenerComentariosAsync(
        int incidenciaId, CancellationToken ct = default) =>
        await _db.ComentariosIncidencia
            .AsNoTracking()
            .Include(c => c.Usuario)
            .Where(c => c.IncidenciaId == incidenciaId)
            .OrderBy(c => c.FechaComentario)
            .ToListAsync(ct);
}