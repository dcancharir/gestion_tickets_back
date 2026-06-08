using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ValoracionRepository : IValoracionRepository {
    private readonly ApplicationDbContext _db;

    public ValoracionRepository(ApplicationDbContext db) => _db = db;

    public async Task<ValoracionTicket?> ObtenerPorIncidenciaPublicIdAsync(
        Guid publicId, CancellationToken ct = default) {

        return await _db.ValoracionesTicket
            .AsNoTracking()
            .Include(v => v.Incidencia)
            .FirstOrDefaultAsync(v => v.Incidencia.PublicId == publicId, ct);
    }

    public async Task<ValoracionTicket> CrearAsync(
        ValoracionTicket valoracion, CancellationToken ct = default) {

        _db.ValoracionesTicket.Add(valoracion);
        await _db.SaveChangesAsync(ct);
        return valoracion;
    }

    public async Task<bool> ExistePorIncidenciaIdAsync(
        int incidenciaId, CancellationToken ct = default) {

        return await _db.ValoracionesTicket
            .AnyAsync(v => v.IncidenciaId == incidenciaId, ct);
    }
}
