using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository {
    private readonly ApplicationDbContext _db;

    public UsuarioRepository(ApplicationDbContext db) => _db = db;

    public async Task<IEnumerable<Usuario>> ObtenerTodosAsync(CancellationToken ct = default) =>
        await _db.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .OrderBy(u => u.Apellidos).ThenBy(u => u.Nombre)
            .ToListAsync(ct);

    public async Task<Usuario?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        await _db.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.UsuarioId == id, ct);

    // Búsqueda por GUID — usada desde los endpoints públicos
    public async Task<Usuario?> ObtenerPorPublicIdAsync(Guid publicId, CancellationToken ct = default) =>
        await _db.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.PublicId == publicId, ct);

    public async Task<Usuario?> ObtenerPorEmailAsync(string email, CancellationToken ct = default) =>
        await _db.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<bool> ExisteEmailAsync(
        string email, int? excluirId = null, CancellationToken ct = default) =>
        await _db.Usuarios.AnyAsync(
            u => u.Email == email && (excluirId == null || u.UsuarioId != excluirId), ct);

    public async Task<Usuario> CrearAsync(Usuario usuario, CancellationToken ct = default) {
        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync(ct);
        return usuario;
    }

    public async Task<Usuario> ActualizarAsync(Usuario usuario, CancellationToken ct = default) {
        _db.Usuarios.Update(usuario);
        await _db.SaveChangesAsync(ct);
        return usuario;
    }

    public async Task EliminarAsync(int id, CancellationToken ct = default) {
        var usuario = await _db.Usuarios.FindAsync(new object[] { id }, ct)
            ?? throw new KeyNotFoundException($"Usuario con Id {id} no encontrado.");
        _db.Usuarios.Remove(usuario);
        await _db.SaveChangesAsync(ct);
    }
}
