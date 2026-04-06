using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class RolConfiguration : IEntityTypeConfiguration<Rol> {
    public void Configure(EntityTypeBuilder<Rol> builder) {
        builder.ToTable("Roles");

        builder.HasKey(r => r.RolId);

        builder.Property(r => r.RolId)
            .UseIdentityColumn();

        builder.Property(r => r.Nombre)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Descripcion)
            .HasMaxLength(255);

        builder.HasIndex(r => r.Nombre)
            .IsUnique();
        
        // ── Seed ─────────────────────────────────────────────────────────────
        builder.HasData(
            new Rol { RolId = 1, Nombre = "Administrador", Descripcion = "Acceso total al sistema" },
            new Rol { RolId = 2, Nombre = "Técnico", Descripcion = "Gestiona y resuelve incidencias" },
            new Rol { RolId = 3, Nombre = "Solicitante", Descripcion = "Registra y da seguimiento a sus tickets" }
        );
    }
}