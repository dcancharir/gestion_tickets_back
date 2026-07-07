using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TicketFavoritoConfiguration : IEntityTypeConfiguration<TicketFavorito> {
    public void Configure(EntityTypeBuilder<TicketFavorito> b) {
        b.HasKey(f => new { f.UsuarioId, f.IncidenciaId });

        b.HasOne(f => f.Usuario)
         .WithMany()
         .HasForeignKey(f => f.UsuarioId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(f => f.Incidencia)
         .WithMany()
         .HasForeignKey(f => f.IncidenciaId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}
