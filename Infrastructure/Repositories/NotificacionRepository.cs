using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NotificacionRepository : INotificacionRepository {
    private readonly ApplicationDbContext _db;

    public NotificacionRepository(ApplicationDbContext db) => _db = db;

    public async Task<Notificacion> CrearAsync(Notificacion notificacion, CancellationToken ct = default) {
        _db.Notificaciones.Add(notificacion);
        await _db.SaveChangesAsync(ct);
        return notificacion;
    }

    public async Task<IEnumerable<Notificacion>> ObtenerNoLeidasPorUsuarioAsync(
        int usuarioId, CancellationToken ct = default) =>
        await _db.Notificaciones
            .Where(n => n.UsuarioId == usuarioId && !n.Leida)
            .OrderByDescending(n => n.FechaCreacion)
            .ToListAsync(ct);

    public async Task MarcarComoLeidaAsync(
        int notificacionId, int usuarioId, CancellationToken ct = default) =>
        await _db.Notificaciones
            .Where(n => n.NotificacionId == notificacionId && n.UsuarioId == usuarioId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(n => n.Leida, true)
                .SetProperty(n => n.FechaLectura, DateTime.Now), ct);

    public async Task MarcarTodasLeidasAsync(int usuarioId, CancellationToken ct = default) =>
        await _db.Notificaciones
            .Where(n => n.UsuarioId == usuarioId && !n.Leida)
            .ExecuteUpdateAsync(s => s
                .SetProperty(n => n.Leida, true)
                .SetProperty(n => n.FechaLectura, DateTime.Now), ct);
}
