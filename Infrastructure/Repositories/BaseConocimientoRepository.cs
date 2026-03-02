using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class BaseConocimientoRepository : IBaseConocimientoRepository {
    private readonly ApplicationDbContext _db;
    public BaseConocimientoRepository(ApplicationDbContext db) => _db = db;

    public async Task<IEnumerable<BaseConocimiento>> ObtenerTodosAsync(
        bool soloActivos = true, CancellationToken ct = default) =>
        await _db.BaseConocimiento
            .AsNoTracking()
            .Include(b => b.Categoria)
            .Include(b => b.CreadoPor)
            .Where(b => !soloActivos || b.Activo)
            .OrderByDescending(b => b.FechaCreacion)
            .ToListAsync(ct);

    public async Task<BaseConocimiento?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        await _db.BaseConocimiento
            .AsNoTracking()
            .Include(b => b.Categoria)
            .Include(b => b.CreadoPor)
            .FirstOrDefaultAsync(b => b.ArticuloId == id, ct);

    public async Task<BaseConocimiento?> ObtenerPorPublicIdAsync(Guid publicId, CancellationToken ct = default) =>
        await _db.BaseConocimiento
            .AsNoTracking()
            .Include(b => b.Categoria)
            .Include(b => b.CreadoPor)
            .FirstOrDefaultAsync(b => b.PublicId == publicId, ct);

    public async Task<IEnumerable<BaseConocimiento>> BuscarAsync(
        string termino, CancellationToken ct = default) =>
        await _db.BaseConocimiento
            .AsNoTracking()
            .Include(b => b.Categoria)
            .Include(b => b.CreadoPor)
            .Where(b => b.Activo &&
                       (b.Titulo.Contains(termino) ||
                        b.Problema.Contains(termino) ||
                        b.Solucion.Contains(termino)))
            .OrderByDescending(b => b.FechaCreacion)
            .ToListAsync(ct);

    public async Task<IEnumerable<BaseConocimiento>> ObtenerPorCategoriaAsync(
        int categoriaId, CancellationToken ct = default) =>
        await _db.BaseConocimiento
            .AsNoTracking()
            .Include(b => b.Categoria)
            .Include(b => b.CreadoPor)
            .Where(b => b.Activo && b.CategoriaId == categoriaId)
            .OrderByDescending(b => b.FechaCreacion)
            .ToListAsync(ct);

    public async Task<BaseConocimiento> CrearAsync(BaseConocimiento articulo, CancellationToken ct = default) {
        _db.BaseConocimiento.Add(articulo);
        await _db.SaveChangesAsync(ct);
        return articulo;
    }

    public async Task<BaseConocimiento> ActualizarAsync(BaseConocimiento articulo, CancellationToken ct = default) {
        _db.BaseConocimiento.Update(articulo);
        await _db.SaveChangesAsync(ct);
        return articulo;
    }

    public async Task EliminarAsync(int id, CancellationToken ct = default) {
        var articulo = await _db.BaseConocimiento.FindAsync(new object[] { id }, ct)
            ?? throw new KeyNotFoundException($"Artículo con Id {id} no encontrado.");
        _db.BaseConocimiento.Remove(articulo);
        await _db.SaveChangesAsync(ct);
    }
}
