using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // ── DbSets ───────────────────────────────────────────────────────────────
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<NivelPrioridad> NivelesPrioridad { get; set; }
    public DbSet<EstadoIncidencia> EstadosIncidencia { get; set; }
    public DbSet<AcuerdoNivelServicio> AcuerdosNivelServicio { get; set; }
    public DbSet<Incidencia> Incidencias { get; set; }
    public DbSet<HistorialIncidencia> HistorialIncidencias { get; set; }
    public DbSet<ComentarioIncidencia> ComentariosIncidencia { get; set; }
    public DbSet<BaseConocimiento> BaseConocimiento { get; set; }
    
    public DbSet<PermisoRol> PermisosRol { get; set; }
    public DbSet<Permiso> Permisos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        // Aplica automáticamente todas las clases IEntityTypeConfiguration<T>
        // que estén en el mismo ensamblado que este DbContext.
        // Cada vez que añadas una nueva Configuration, se registra sola.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    // ── Intercepción de SaveChanges ──────────────────────────────────────────
    // Actualiza FechaUltimaActualizacion automáticamente en cada guardado.
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        ActualizarFechaModificacion();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges() {
        ActualizarFechaModificacion();
        return base.SaveChanges();
    }

    private void ActualizarFechaModificacion() {
        var entradas = ChangeTracker.Entries<Incidencia>()
            .Where(e => e.State == EntityState.Modified);

        foreach(var entrada in entradas)
            entrada.Entity.FechaUltimaActualizacion = DateTime.Now;
    }
}
