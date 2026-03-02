using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class HistorialIncidenciaConfiguration : IEntityTypeConfiguration<HistorialIncidencia> {
    public void Configure(EntityTypeBuilder<HistorialIncidencia> builder) {
        builder.ToTable("HistorialIncidencias");

        builder.HasKey(h => h.HistorialId);

        builder.Property(h => h.HistorialId)
            .UseIdentityColumn();

        builder.Property(h => h.Accion)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(h => h.EstadoAnterior)
            .HasMaxLength(50);

        builder.Property(h => h.EstadoNuevo)
            .HasMaxLength(50);

        builder.Property(h => h.Detalle)
            .HasColumnType("nvarchar(max)");

        builder.Property(h => h.FechaAccion)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        // ── Índice ────────────────────────────────────────────────────────────
        builder.HasIndex(h => h.IncidenciaId);

        // ── Relaciones ────────────────────────────────────────────────────────

        // Cascade: al eliminar una incidencia se elimina su historial completo
        builder.HasOne(h => h.Incidencia)
            .WithMany(i => i.Historial)
            .HasForeignKey(h => h.IncidenciaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restrict: no se puede eliminar un usuario que tenga acciones en el historial
        builder.HasOne(h => h.Usuario)
            .WithMany(u => u.HistorialAcciones)
            .HasForeignKey(h => h.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
