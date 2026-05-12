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
        string termino, CancellationToken ct = default) {

        // ── 1. Normalizar: dividir en palabras significativas (≥ 3 caracteres) ──
        //    "en sala el quiosco" → ["sala", "quiosco"]  (ignora "en", "el")
        var palabras = termino
            .Trim()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(p => p.Length >= 3)
            .Distinct()
            .ToArray();

        if(palabras.Length == 0)
            return Enumerable.Empty<BaseConocimiento>();

        var queryBase = _db.BaseConocimiento
            .AsNoTracking()
            .Include(b => b.Categoria)
            .Include(b => b.CreadoPor)
            .Where(b => b.Activo);

        // ── 2. Intento AND: todas las palabras deben aparecer en algún campo ──
        //    Cada .Where() encadenado agrega un AND en SQL
        var queryAnd = queryBase;
        foreach(var palabra in palabras) {
            var p = palabra; // captura para el closure de EF
            queryAnd = queryAnd.Where(b =>
                b.Titulo.Contains(p) ||
                b.Problema.Contains(p) ||
                b.Solucion.Contains(p));
        }

        var resultados = await queryAnd.ToListAsync(ct);

        // ── 3. Fallback OR: si AND no dio resultados, basta con que aparezca
        //    al menos UNA palabra (se filtra en memoria, la KB es pequeña)  ──
        if(!resultados.Any()) {
            var todos = await queryBase.ToListAsync(ct);
            resultados = todos
                .Where(b => palabras.Any(p =>
                    b.Titulo.Contains(p, StringComparison.OrdinalIgnoreCase)   ||
                    b.Problema.Contains(p, StringComparison.OrdinalIgnoreCase) ||
                    b.Solucion.Contains(p, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        // ── 4. Ordenar por relevancia ──────────────────────────────────────────
        //    Criterio 1: cuántas palabras aparecen en el título (más = primero)
        //    Criterio 2: cuántas aparecen en problema o solución
        //    Criterio 3: más reciente
        return resultados
            .OrderByDescending(b => palabras.Count(p =>
                b.Titulo.Contains(p, StringComparison.OrdinalIgnoreCase)))
            .ThenByDescending(b => palabras.Count(p =>
                b.Problema.Contains(p, StringComparison.OrdinalIgnoreCase) ||
                b.Solucion.Contains(p, StringComparison.OrdinalIgnoreCase)))
            .ThenByDescending(b => b.FechaCreacion);
    }

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
