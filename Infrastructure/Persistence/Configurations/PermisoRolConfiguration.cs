using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Persistence.Configurations;

public class PermisoRolConfiguration : IEntityTypeConfiguration<PermisoRol>
{
    public void Configure(EntityTypeBuilder<PermisoRol> builder)
    {
        builder.ToTable("PermisosRol");
        
        builder.HasKey(r=>r.PermisoRolId);
        
        builder.Property(r=>r.PermisoRolId).UseIdentityColumn();
        
       

        builder.HasOne(u => u.Rol)
            .WithMany(r => r.PermisoRoles)
            .HasForeignKey(u => u.RolId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(u => u.Permiso)
            .WithMany(r => r.PermisoRoles)
            .HasForeignKey(u => u.PermisoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}