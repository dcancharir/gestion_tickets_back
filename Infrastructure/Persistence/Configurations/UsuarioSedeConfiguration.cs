using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Configurations;

public class UsuarioSedeConfiguration : IEntityTypeConfiguration<UsuarioSede> {
    public void Configure(EntityTypeBuilder<UsuarioSede> builder) {

        builder.ToTable("UsuarioSede");

        builder.HasKey(u => u.UsuarioSedeId);

        builder.HasOne(u => u.Usuario)
          .WithMany(r => r.UsuarioSedes)
          .HasForeignKey(u => u.UsuarioId)
          .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(u => u.Sede)
        .WithMany(r => r.UsuarioSedes)
        .HasForeignKey(u => u.SedeId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
