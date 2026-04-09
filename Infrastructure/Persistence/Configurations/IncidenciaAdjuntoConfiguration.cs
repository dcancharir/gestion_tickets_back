using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class IncidenciaAdjuntoConfiguration : IEntityTypeConfiguration<IncidenciaAdjunto>
{
    public void Configure(EntityTypeBuilder<IncidenciaAdjunto> builder)
    {
        builder.ToTable("IncidenciaAdjuntos");

        builder.HasKey(i => i.IncidenciaAdjuntoId);

        builder.Property(i => i.IncidenciaAdjuntoId)
            .UseIdentityColumn();
        
        builder.Property(i => i.Nombre)
            .HasColumnType("nvarchar(250)");
        builder.Property(i => i.NombreReal)
            .HasColumnType("nvarchar(max)");
        builder.Property(i => i.RutaContenedora)
            .HasColumnType("nvarchar(max)");
        
        // Cascade: al eliminar una incidencia se elimina sus adjuntos completo
        builder.HasOne(h => h.Incidencia)
            .WithMany(i => i.Adjuntos)
            .HasForeignKey(h => h.IncidenciaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
