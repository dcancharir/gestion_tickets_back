using Application.Ports.Driven;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class SedeRepository : ISedeRepository {
    private readonly ApplicationDbContext _db;
    public SedeRepository(ApplicationDbContext db) {
        _db = db;
    }
    public async Task<Sede> CrearAsync(Sede sede, CancellationToken ct) {
        await _db.Sedes.AddAsync(sede, ct);
        await _db.SaveChangesAsync(ct);
        return sede;
    }

    public async Task<IEnumerable<Sede>> ObtenerTodasAsync(CancellationToken ct) => 
        await _db.Sedes.AsNoTracking().ToListAsync();
}
