using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario> {
    public void Configure(EntityTypeBuilder<Usuario> builder) {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.UsuarioId);

        builder.Property(u => u.UsuarioId)
            .UseIdentityColumn();

        // PublicId: generado por SQL Server con NEWSEQUENTIALID()
        // Secuencial → no fragmenta el índice clustered como NEWID()
        builder.Property(u => u.PublicId)
            .IsRequired()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        // Índice único sobre PublicId para búsquedas rápidas desde el frontend
        builder.HasIndex(u => u.PublicId)
            .IsUnique();

        builder.Property(u => u.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Apellidos)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Activo)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.FechaCreacion)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(u => u.UserName)
            .IsUnique();

        builder.HasOne(u => u.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.RolId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
