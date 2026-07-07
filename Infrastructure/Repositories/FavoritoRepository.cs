using Application.DTOS.Favoritos;
using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FavoritoRepository : IFavoritoRepository {
    private readonly ApplicationDbContext _db;
    public FavoritoRepository(ApplicationDbContext db) => _db = db;

    public async Task<IReadOnlyList<FavoritoDto>> ObtenerAsync(int usuarioId, CancellationToken ct = default) =>
        await _db.TicketsFavoritos
            .AsNoTracking()
            .Where(f => f.UsuarioId == usuarioId)
            .Include(f => f.Incidencia).ThenInclude(i => i.Categoria)
            .OrderByDescending(f => f.FechaAgregado)
            .Select(f => new FavoritoDto(
                f.IncidenciaId,
                f.Incidencia.PublicId,
                f.Incidencia.NumeroTicket,
                f.Incidencia.Titulo,
                f.Incidencia.Descripcion,
                f.Incidencia.CategoriaId,
                f.Incidencia.Categoria.Nombre,
                f.Incidencia.CanalReporte,
                f.Incidencia.Impacto,
                f.Incidencia.Urgencia,
                f.Incidencia.SedeId
            ))
            .ToListAsync(ct);

    private const int MaxFavoritos = 10;

    public async Task AgregarAsync(int usuarioId, Guid publicId, CancellationToken ct = default) {
        var incidenciaId = await _db.Incidencias
            .Where(i => i.PublicId == publicId)
            .Select(i => i.IncidenciaId)
            .FirstOrDefaultAsync(ct);
        if (incidenciaId == 0) return;
        if (await _db.TicketsFavoritos.AnyAsync(f => f.UsuarioId == usuarioId && f.IncidenciaId == incidenciaId, ct)) return;

        var count = await _db.TicketsFavoritos.CountAsync(f => f.UsuarioId == usuarioId, ct);
        if (count >= MaxFavoritos)
            throw new Application.Exceptions.ValidationException($"No puedes tener más de {MaxFavoritos} tickets favoritos.");

        _db.TicketsFavoritos.Add(new TicketFavorito { UsuarioId = usuarioId, IncidenciaId = incidenciaId });
        await _db.SaveChangesAsync(ct);
    }

    public async Task EliminarAsync(int usuarioId, Guid publicId, CancellationToken ct = default) {
        var incidenciaId = await _db.Incidencias
            .Where(i => i.PublicId == publicId)
            .Select(i => i.IncidenciaId)
            .FirstOrDefaultAsync(ct);
        if (incidenciaId == 0) return;
        var fav = await _db.TicketsFavoritos.FindAsync([usuarioId, incidenciaId], ct);
        if (fav is null) return;
        _db.TicketsFavoritos.Remove(fav);
        await _db.SaveChangesAsync(ct);
    }
}
