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

        builder.Property(u => u.HasFullAccess).HasDefaultValue(false);

        builder.Property(u => u.TokenRecuperacion)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(u => u.TokenExpiracion)
            .IsRequired(false);

        builder.HasOne(u => u.Rol)
            .WithMany(r => r.Usuarios)
            .HasForeignKey(u => u.RolId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasData(
            new Usuario() { UsuarioId=1, PublicId= new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC"), FechaCreacion= new DateTime(2026, 5, 10), RolId=1, Activo = true, Apellidos = "Canchari",Nombre ="Diego", Email = "diego.canchari@designdevsoftware.com",UserName = "d.cancharir",PasswordHash= "$2a$11$2oABfgbGT3nu0gKBQ1C4h.uncd85k9GNxndr8ehlu.yGmtkMhBFse",HasFullAccess = true },//password : 102030
            new Usuario() { UsuarioId=2, PublicId= new Guid("716578A3-4371-4894-A63D-435E4C0CD3B8"),FechaCreacion= new DateTime(2026, 5, 10), RolId=1, Activo = true, Apellidos = "Perez",Nombre ="Juan", Email = "diego.canchari@designdevsoftware.com",UserName = "administrador",PasswordHash= "$2a$11$2oABfgbGT3nu0gKBQ1C4h.uncd85k9GNxndr8ehlu.yGmtkMhBFse",HasFullAccess = false },//password : 102030
            new Usuario() { UsuarioId=3, PublicId= new Guid("507DA209-C3A9-45E3-AAFF-3CF7D334B125"),FechaCreacion= new DateTime(2026, 5, 10), RolId=1, Activo = true, Apellidos = "Fernandez",Nombre ="Carlos", Email = "diego.canchari@designdevsoftware.com",UserName = "tecnico",PasswordHash= "$2a$11$2oABfgbGT3nu0gKBQ1C4h.uncd85k9GNxndr8ehlu.yGmtkMhBFse",HasFullAccess = false },//password : 102030
            new Usuario() { UsuarioId=4, PublicId= new Guid("4CFA9C79-62B8-49CD-AEF7-70F593511D6D"),FechaCreacion=new DateTime(2026, 5, 10), RolId=1, Activo = true, Apellidos = "Lopez",Nombre ="Mario", Email = "diego.canchari@designdevsoftware.com",UserName = "solicitante",PasswordHash= "$2a$11$2oABfgbGT3nu0gKBQ1C4h.uncd85k9GNxndr8ehlu.yGmtkMhBFse",HasFullAccess = false }//password : 102030
            );
    }
}
