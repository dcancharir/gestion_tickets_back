using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class CategoriaRepository : ICategoriaRepository {
    private readonly ApplicationDbContext _db;
    public CategoriaRepository(ApplicationDbContext db) => _db = db;

    public async Task<IEnumerable<Categoria>> ObtenerTodasAsync(CancellationToken ct = default) =>
        await _db.Categorias.AsNoTracking().OrderBy(c => c.Nombre).ToListAsync(ct);

    public async Task<IEnumerable<Categoria>> ObtenerActivasAsync(CancellationToken ct = default) =>
        await _db.Categorias.AsNoTracking().Where(c => c.Activo).OrderBy(c => c.Nombre).ToListAsync(ct);

    public async Task<Categoria?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        await _db.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.CategoriaId == id, ct);

    public async Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null, CancellationToken ct = default) =>
        await _db.Categorias.AnyAsync(c => c.Nombre == nombre && (excluirId == null || c.CategoriaId != excluirId), ct);

    public async Task<Categoria> CrearAsync(Categoria categoria, CancellationToken ct = default) {
        _db.Categorias.Add(categoria);
        await _db.SaveChangesAsync(ct);
        return categoria;
    }

    public async Task<Categoria> ActualizarAsync(Categoria categoria, CancellationToken ct = default) {
        _db.Categorias.Update(categoria);
        await _db.SaveChangesAsync(ct);
        return categoria;
    }

    public async Task EliminarAsync(int id, CancellationToken ct = default) {
        var categoria = await _db.Categorias.FindAsync(new object[] { id }, ct)
            ?? throw new KeyNotFoundException($"Categoría con Id {id} no encontrada.");
        _db.Categorias.Remove(categoria);
        await _db.SaveChangesAsync(ct);
    }
}