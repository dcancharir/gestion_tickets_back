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

        //builder.HasIndex(n => n.Nombre)
        //    .IsUnique();

        builder.ToTable(t => t.HasCheckConstraint("CK_NivelPrioridad_Nivel", "[Nivel] BETWEEN 1 AND 4"));

        // ── Seed ─────────────────────────────────────────────────────────────
        builder.HasData(
            new NivelPrioridad { PrioridadId = 1,Impacto = 1,Urgencia=1,CssClass="danger",ColorHexa= "#B91C1C", Descripcion= "Impacto Alto + Urgencia Alta",GuiaValor= "ATENCIÓN INMEDIATA - El valor del servicio está severamente comprometido. Asignar recursos prioritarios y mantener comunicación constante con stakeholders.", Nombre = "Crítico", Nivel = 1, TiempoRespuestaMin = 15, TiempoResolucionMin = 60 },
            new NivelPrioridad { PrioridadId = 2,Impacto = 1,Urgencia=2,CssClass="warning",ColorHexa= "#EA580C", Descripcion= "Impacto Alto + Urgencia Media",GuiaValor= "ALTA PRIORIDAD - Valor del servicio significativamente afectado. Coordinar recursos y establecer plan de acción claro con tiempos definidos.", Nombre = "Alto", Nivel = 2, TiempoRespuestaMin = 30, TiempoResolucionMin = 240 },
            new NivelPrioridad { PrioridadId = 3,Impacto = 2,Urgencia=1,CssClass="warning", ColorHexa= "#EA580C", Descripcion= "Impacto Medio + Urgencia Alta",GuiaValor= "ALTA PRIORIDAD - Requiere respuesta rápida aunque el impacto sea limitado. Asegurar que el usuario pueda continuar operaciones críticas.", Nombre = "Alto", Nivel = 2, TiempoRespuestaMin = 30, TiempoResolucionMin = 240 },
            new NivelPrioridad { PrioridadId = 4,Impacto = 1,Urgencia=3,CssClass="info", ColorHexa= "#2563EB", Descripcion= "Impacto Alto + Urgencia Baja",GuiaValor= "PRIORIDAD MEDIA - Planificar resolución considerando impacto potencial. Puede coordinarse con otros cambios planificados.", Nombre = "Medio", Nivel = 3, TiempoRespuestaMin = 60, TiempoResolucionMin = 480 },
            new NivelPrioridad { PrioridadId = 5,Impacto = 2,Urgencia=2,CssClass="info",ColorHexa= "#2563EB", Descripcion= "Impacto Medio + Urgencia Media",GuiaValor= "PRIORIDAD MEDIA - Gestión normal siguiendo procedimientos estándar. Mantener al usuario informado del progreso.", Nombre = "Medio", Nivel = 3, TiempoRespuestaMin = 60, TiempoResolucionMin = 480},
            new NivelPrioridad { PrioridadId = 6,Impacto = 3,Urgencia=1,CssClass="info",ColorHexa= "#2563EB", Descripcion= "Impacto Bajo + Urgencia Alta",GuiaValor= "PRIORIDAD MEDIA - Respuesta rápida con bajo impacto. Buscar soluciones rápidas o workarounds temporales.", Nombre = "Medio", Nivel = 3, TiempoRespuestaMin = 60, TiempoResolucionMin = 480 },
            new NivelPrioridad { PrioridadId = 7,Impacto = 2,Urgencia=3,CssClass="success",ColorHexa= "#15803D", Descripcion= "Impacto Medio + Urgencia Baja",GuiaValor= "BAJA PRIORIDAD - Gestionar en backlog normal. Puede agruparse con otras tareas similares para eficiencia.", Nombre = "Bajo", Nivel = 4, TiempoRespuestaMin = 120, TiempoResolucionMin = 1440 },
            new NivelPrioridad { PrioridadId = 8,Impacto = 3,Urgencia=2,CssClass="success",ColorHexa= "#15803D", Descripcion= "Impacto Bajo + Urgencia Media",GuiaValor= "BAJA PRIORIDAD - Resolver cuando sea posible. Mantener comunicación básica con usuario.", Nombre = "Bajo", Nivel = 4, TiempoRespuestaMin = 120, TiempoResolucionMin = 1440 },
            new NivelPrioridad { PrioridadId = 9,Impacto = 3,Urgencia=3,CssClass="success",ColorHexa= "#15803D", Descripcion= "Impacto Bajo + Urgencia Baja",GuiaValor= "BAJA PRIORIDAD - Mínimo impacto en el valor. Puede postergarse si surgen prioridades mayores.", Nombre = "Bajo", Nivel = 4, TiempoRespuestaMin = 120, TiempoResolucionMin = 1440}
        );
    }
}
