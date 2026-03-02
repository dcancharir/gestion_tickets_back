using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class BaseConocimientoConfiguration : IEntityTypeConfiguration<BaseConocimiento> {
    public void Configure(EntityTypeBuilder<BaseConocimiento> builder) {
        builder.ToTable("BaseConocimiento");

        builder.HasKey(b => b.ArticuloId);

        builder.Property(b => b.PublicId)
            .IsRequired()
            .HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.HasIndex(b => b.PublicId).IsUnique();
        builder.Property(b => b.ArticuloId)
            .UseIdentityColumn();

        builder.Property(b => b.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Problema)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(b => b.Solucion)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(b => b.FechaCreacion)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(b => b.Activo)
            .IsRequired()
            .HasDefaultValue(true);

        // ── Relaciones ────────────────────────────────────────────────────────

        // SetNull: si se elimina la categoría, el artículo se conserva sin categoría
        builder.HasOne(b => b.Categoria)
            .WithMany(c => c.ArticulosConocimiento)
            .HasForeignKey(b => b.CategoriaId)
            .OnDelete(DeleteBehavior.SetNull);

        // Restrict: no se puede eliminar un usuario que haya creado artículos
        builder.HasOne(b => b.CreadoPor)
            .WithMany(u => u.ArticulosCreados)
            .HasForeignKey(b => b.CreadoPorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
