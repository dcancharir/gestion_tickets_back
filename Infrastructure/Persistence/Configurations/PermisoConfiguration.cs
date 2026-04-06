using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
{
    public void Configure(EntityTypeBuilder<Permiso> builder)
    {
        builder.ToTable("Permisos");
        
        builder.HasKey(p => p.PermisoId);
        builder.Property(p => p.PermisoId).UseIdentityColumn();
        builder.Property(p => p.Nombre).HasMaxLength(250).IsRequired();
        builder.Property(p => p.Tipo).HasMaxLength(250).IsRequired();
        builder.Property(p => p.Controlador).HasMaxLength(250).IsRequired();
    }
}