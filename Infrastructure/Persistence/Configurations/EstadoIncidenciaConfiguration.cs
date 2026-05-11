using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class EstadoIncidenciaConfiguration : IEntityTypeConfiguration<EstadoIncidencia> {
    public void Configure(EntityTypeBuilder<EstadoIncidencia> builder) {
        builder.ToTable("EstadosIncidencia");

        builder.HasKey(e => e.EstadoId);

        builder.Property(e => e.EstadoId)
            .UseIdentityColumn();

        builder.Property(e => e.Nombre)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Descripcion)
            .HasMaxLength(255);

        builder.Property(e => e.EsEstadoFinal)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(e => e.Nombre)
            .IsUnique();

        // ── Seed ─────────────────────────────────────────────────────────────
        builder.HasData(
            new EstadoIncidencia { EstadoId = 1,CssClass= "secondary", ColorHexa= "#6C757D", Nombre = "Registrado", Descripcion = "Incidencia recibida y pendiente de asignación", EsEstadoFinal = false },
            new EstadoIncidencia { EstadoId = 2,CssClass= "primary", ColorHexa= "#0D6EFD", Nombre = "Asignado", Descripcion = "Asignado a un técnico, pendiente de atención", EsEstadoFinal = false },
            new EstadoIncidencia { EstadoId = 3,CssClass= "info", ColorHexa= "#0DCAF0", Nombre = "En Diagnóstico", Descripcion = "El técnico está analizando la causa raíz", EsEstadoFinal = false },
            new EstadoIncidencia { EstadoId = 4,CssClass= "warning", ColorHexa= "#FFC107", Nombre = "En Progreso", Descripcion = "Se está aplicando la solución", EsEstadoFinal = false },
            new EstadoIncidencia { EstadoId = 5,CssClass= "dark", ColorHexa= "#495057", Nombre = "Pendiente", Descripcion = "En espera de respuesta del solicitante o de un proveedor", EsEstadoFinal = false },
            new EstadoIncidencia { EstadoId = 6,CssClass= "success", ColorHexa= "#198754", Nombre = "Resuelto", Descripcion = "Solución aplicada, pendiente de confirmación del usuario", EsEstadoFinal = false },
            new EstadoIncidencia { EstadoId = 7,CssClass= "secondary", ColorHexa= "#ADB5BD", Nombre = "Cerrado", Descripcion = "Confirmado y cerrado formalmente", EsEstadoFinal = true },
            new EstadoIncidencia { EstadoId = 8,CssClass= "danger", ColorHexa= "#DC3545", Nombre = "Reabierto", Descripcion = "El usuario reportó que el problema persiste", EsEstadoFinal = false },
            new EstadoIncidencia { EstadoId = 9,CssClass= "light", ColorHexa= "#E9ECEF", Nombre = "Cancelado", Descripcion = "Incidencia anulada por el solicitante o administrador", EsEstadoFinal = true }
        );
    }
}

