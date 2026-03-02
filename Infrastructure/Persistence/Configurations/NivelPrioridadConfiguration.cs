using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class NivelPrioridadConfiguration : IEntityTypeConfiguration<NivelPrioridad> {
    public void Configure(EntityTypeBuilder<NivelPrioridad> builder) {
        builder.ToTable("NivelesPrioridad");

        builder.HasKey(n => n.PrioridadId);

        builder.Property(n => n.PrioridadId)
            .UseIdentityColumn();

        builder.Property(n => n.Nombre)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(n => n.Nivel)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(n => n.TiempoRespuestaMin)
            .IsRequired();

        builder.Property(n => n.TiempoResolucionMin)
            .IsRequired();

        builder.HasIndex(n => n.Nombre)
            .IsUnique();

        builder.ToTable(t => t.HasCheckConstraint("CK_NivelPrioridad_Nivel", "[Nivel] BETWEEN 1 AND 5"));

        // ── Seed ─────────────────────────────────────────────────────────────
        builder.HasData(
            new NivelPrioridad { PrioridadId = 1, Nombre = "Crítico", Nivel = 1, TiempoRespuestaMin = 15, TiempoResolucionMin = 60 },
            new NivelPrioridad { PrioridadId = 2, Nombre = "Alto", Nivel = 2, TiempoRespuestaMin = 30, TiempoResolucionMin = 240 },
            new NivelPrioridad { PrioridadId = 3, Nombre = "Medio", Nivel = 3, TiempoRespuestaMin = 60, TiempoResolucionMin = 480 },
            new NivelPrioridad { PrioridadId = 4, Nombre = "Bajo", Nivel = 4, TiempoRespuestaMin = 120, TiempoResolucionMin = 1440 },
            new NivelPrioridad { PrioridadId = 5, Nombre = "Planificado", Nivel = 5, TiempoRespuestaMin = 480, TiempoResolucionMin = 4320 }
        );
    }
}
