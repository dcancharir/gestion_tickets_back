using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria> {
    public void Configure(EntityTypeBuilder<Categoria> builder) {
        builder.ToTable("Categorias");

        builder.HasKey(c => c.CategoriaId);

        builder.Property(c => c.CategoriaId)
            .UseIdentityColumn();

        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Descripcion)
            .HasMaxLength(255);

        builder.Property(c => c.Activo)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(c => c.Nombre)
            .IsUnique();

        // ── Seed ─────────────────────────────────────────────────────────────
        builder.HasData(
            new Categoria { CategoriaId = 1, Nombre = "Hardware", Descripcion = "Fallas en equipos físicos: computadoras, impresoras, periféricos" },
            new Categoria { CategoriaId = 2, Nombre = "Software", Descripcion = "Errores en aplicaciones, sistemas operativos o instalaciones" },
            new Categoria { CategoriaId = 3, Nombre = "Red y Conectividad", Descripcion = "Problemas de internet, red local, VPN o switches" },
            new Categoria { CategoriaId = 4, Nombre = "Seguridad", Descripcion = "Virus, accesos no autorizados, vulnerabilidades" },
            new Categoria { CategoriaId = 5, Nombre = "Correo Electrónico", Descripcion = "Fallos en cuentas de correo, Outlook o servidor de mail" },
            new Categoria { CategoriaId = 6, Nombre = "Base de Datos", Descripcion = "Errores en motores de base de datos o consultas" },
            new Categoria { CategoriaId = 7, Nombre = "Accesos y Permisos", Descripcion = "Solicitudes de acceso, bloqueos de cuenta o cambios de contraseña" },
            new Categoria { CategoriaId = 8, Nombre = "Otros", Descripcion = "Incidencias que no corresponden a las categorías anteriores" }
        );
    }
}