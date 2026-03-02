using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class IncidenciaConfiguration : IEntityTypeConfiguration<Incidencia> {
    public void Configure(EntityTypeBuilder<Incidencia> builder) {
        builder.ToTable("Incidencias");

        builder.HasKey(i => i.IncidenciaId);

        builder.Property(i => i.IncidenciaId)
            .UseIdentityColumn();

        // GUID secuencial generado por SQL Server
        builder.Property(i => i.PublicId)
            .IsRequired()
            .HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.HasIndex(i => i.PublicId)
            .IsUnique();

        builder.Property(i => i.NumeroTicket)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(i => i.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.Descripcion)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(i => i.CanalReporte)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.FechaRegistro)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(i => i.Impacto)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(i => i.Urgencia)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.ToTable(t => {
            t.HasCheckConstraint("CK_Incidencia_Impacto", "[Impacto] BETWEEN 1 AND 3");
            t.HasCheckConstraint("CK_Incidencia_Urgencia", "[Urgencia] BETWEEN 1 AND 3");
        });

        builder.Property(i => i.NumeroReasignaciones)
            .IsRequired()
            .HasColumnType("tinyint")
            .HasDefaultValue((byte)0);

        builder.Property(i => i.SolucionAplicada)
            .HasColumnType("nvarchar(max)");

        builder.Property(i => i.ResueltoEnPrimerContacto)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(i => i.FechaUltimaActualizacion)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(i => i.NumeroTicket).IsUnique();
        builder.HasIndex(i => i.EstadoId);
        builder.HasIndex(i => i.TecnicoAsignadoId);
        builder.HasIndex(i => i.SolicitanteId);
        builder.HasIndex(i => new { i.FechaRegistro, i.FechaResolucion, i.FechaCierre })
            .HasDatabaseName("IX_Incidencias_Fechas");

        builder.HasOne(i => i.Solicitante)
            .WithMany(u => u.IncidenciasComoSolicitante)
            .HasForeignKey(i => i.SolicitanteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.TecnicoAsignado)
            .WithMany(u => u.IncidenciasComoTecnico)
            .HasForeignKey(i => i.TecnicoAsignadoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.EscaladoA)
            .WithMany(u => u.IncidenciasEscaladas)
            .HasForeignKey(i => i.EscaladoAId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.CerradoPor)
            .WithMany(u => u.IncidenciasCerradas)
            .HasForeignKey(i => i.CerradoPorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Categoria)
            .WithMany(c => c.Incidencias)
            .HasForeignKey(i => i.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.NivelPrioridad)
            .WithMany(n => n.Incidencias)
            .HasForeignKey(i => i.PrioridadId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.EstadoIncidencia)
            .WithMany(e => e.Incidencias)
            .HasForeignKey(i => i.EstadoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
