using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class AcuerdoNivelServicioConfiguration : IEntityTypeConfiguration<AcuerdoNivelServicio> {
    public void Configure(EntityTypeBuilder<AcuerdoNivelServicio> builder) {
        builder.ToTable("AcuerdosNivelServicio");

        builder.HasKey(s => s.SlaId);

        builder.Property(s => s.SlaId)
            .UseIdentityColumn();

        builder.Property(s => s.TiempoRespuestaMin)
            .IsRequired();

        builder.Property(s => s.TiempoResolucionMin)
            .IsRequired();

        builder.Property(s => s.Activo)
            .IsRequired()
            .HasDefaultValue(true);

        // Índice único: no puede haber dos SLAs para la misma Categoría + Prioridad
        builder.HasIndex(s => new { s.CategoriaId, s.PrioridadId })
            .IsUnique();

        // ── Relaciones ────────────────────────────────────────────────────────
        builder.HasOne(s => s.Categoria)
            .WithMany(c => c.AcuerdosNivelServicio)
            .HasForeignKey(s => s.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.NivelPrioridad)
            .WithMany(n => n.AcuerdosNivelServicio)
            .HasForeignKey(s => s.PrioridadId)
            .OnDelete(DeleteBehavior.Restrict);

        // ── Seed ─────────────────────────────────────────────────────────────
        builder.HasData(
            // Hardware
            new AcuerdoNivelServicio { SlaId = 1, CategoriaId = 1, PrioridadId = 1, TiempoRespuestaMin = 15, TiempoResolucionMin = 60 },
            new AcuerdoNivelServicio { SlaId = 2, CategoriaId = 1, PrioridadId = 2, TiempoRespuestaMin = 30, TiempoResolucionMin = 240 },
            new AcuerdoNivelServicio { SlaId = 3, CategoriaId = 1, PrioridadId = 3, TiempoRespuestaMin = 60, TiempoResolucionMin = 480 },
            // Software
            new AcuerdoNivelServicio { SlaId = 4, CategoriaId = 2, PrioridadId = 1, TiempoRespuestaMin = 15, TiempoResolucionMin = 60 },
            new AcuerdoNivelServicio { SlaId = 5, CategoriaId = 2, PrioridadId = 2, TiempoRespuestaMin = 30, TiempoResolucionMin = 240 },
            new AcuerdoNivelServicio { SlaId = 6, CategoriaId = 2, PrioridadId = 3, TiempoRespuestaMin = 60, TiempoResolucionMin = 480 },
            new AcuerdoNivelServicio { SlaId = 7, CategoriaId = 2, PrioridadId = 4, TiempoRespuestaMin = 120, TiempoResolucionMin = 1440 },
            // Red y Conectividad
            new AcuerdoNivelServicio { SlaId = 8, CategoriaId = 3, PrioridadId = 1, TiempoRespuestaMin = 15, TiempoResolucionMin = 60 },
            new AcuerdoNivelServicio { SlaId = 9, CategoriaId = 3, PrioridadId = 2, TiempoRespuestaMin = 30, TiempoResolucionMin = 240 },
            // Seguridad (SLA más estricto)
            new AcuerdoNivelServicio { SlaId = 10, CategoriaId = 4, PrioridadId = 1, TiempoRespuestaMin = 15, TiempoResolucionMin = 30 },
            new AcuerdoNivelServicio { SlaId = 11, CategoriaId = 4, PrioridadId = 2, TiempoRespuestaMin = 20, TiempoResolucionMin = 120 },
            // Accesos y Permisos
            new AcuerdoNivelServicio { SlaId = 12, CategoriaId = 7, PrioridadId = 3, TiempoRespuestaMin = 60, TiempoResolucionMin = 480 },
            new AcuerdoNivelServicio { SlaId = 13, CategoriaId = 7, PrioridadId = 4, TiempoRespuestaMin = 120, TiempoResolucionMin = 1440 }
        );
    }
}
