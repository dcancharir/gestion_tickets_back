using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UsuarioSedeRepository : IUsuarioSedeRepository {
    private readonly ApplicationDbContext _db;
    public UsuarioSedeRepository(ApplicationDbContext db) => _db = db;

    public async Task<IEnumerable<UsuarioSede>> ObtenerPorUsuarioIdAsync(int usuarioId, CancellationToken ct = default) =>
        await _db.UsuarioSede
            .AsNoTracking()
            .Include(us => us.Sede)
            .Where(us => us.UsuarioId == usuarioId)
            .ToListAsync(ct);

    public async Task AsignarSedesAsync(int usuarioId, IEnumerable<int> sedeIds, CancellationToken ct = default) {
        var existentes = await _db.UsuarioSede
            .Where(us => us.UsuarioId == usuarioId)
            .ToListAsync(ct);

        _db.UsuarioSede.RemoveRange(existentes);

        var nuevas = sedeIds.Select(sedeId => new UsuarioSede { UsuarioId = usuarioId, SedeId = sedeId });
        await _db.UsuarioSede.AddRangeAsync(nuevas, ct);
        await _db.SaveChangesAsync(ct);
    }
}
