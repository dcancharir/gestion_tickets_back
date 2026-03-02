using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class ComentarioIncidenciaConfiguration : IEntityTypeConfiguration<ComentarioIncidencia> {
    public void Configure(EntityTypeBuilder<ComentarioIncidencia> builder) {
        builder.ToTable("ComentariosIncidencia");

        builder.HasKey(c => c.ComentarioId);

        builder.Property(c => c.ComentarioId)
            .UseIdentityColumn();

        builder.Property(c => c.Mensaje)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(c => c.EsInterno)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.FechaComentario)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        // ── Índice ────────────────────────────────────────────────────────────
        builder.HasIndex(c => c.IncidenciaId);

        // ── Relaciones ────────────────────────────────────────────────────────

        // Cascade: al eliminar una incidencia se eliminan sus comentarios
        builder.HasOne(c => c.Incidencia)
            .WithMany(i => i.Comentarios)
            .HasForeignKey(c => c.IncidenciaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restrict: no se puede eliminar un usuario con comentarios registrados
        builder.HasOne(c => c.Usuario)
            .WithMany(u => u.Comentarios)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
