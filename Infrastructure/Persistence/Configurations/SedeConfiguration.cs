using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class SedeConfiguration : IEntityTypeConfiguration<Sede> {
    public void Configure(EntityTypeBuilder<Sede> builder) {
        builder.ToTable("Sedes");

        builder.HasKey(s => s.SedeId);

        builder.Property(s => s.SedeId)
            .UseIdentityColumn();

        builder.Property(i => i.Nombre)
        .HasColumnType("nvarchar(250)");

        builder.Property(i => i.TipoSede)
        .HasColumnType("nvarchar(250)");

        builder.Property(i => i.SedeIdExterno)
            .IsRequired();

        builder.HasData(
            new Sede() {SedeId = 1, SedeIdExterno = 68, Nombre = "DAMASCO", TipoSede = "SALA" },
            new Sede() {SedeId = 2, SedeIdExterno = 36, Nombre = "EXCALIBUR", TipoSede = "SALA"}
        );

    }
}
