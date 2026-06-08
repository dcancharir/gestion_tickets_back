using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ValoracionTicketConfiguration : IEntityTypeConfiguration<ValoracionTicket> {
    public void Configure(EntityTypeBuilder<ValoracionTicket> b) {
        b.HasKey(v => v.ValoracionId);

        b.Property(v => v.Puntuacion).IsRequired();
        b.Property(v => v.Comentario).HasMaxLength(500);
        b.Property(v => v.FechaValoracion).IsRequired();

        // Un ticket solo puede tener una valoración
        b.HasIndex(v => v.IncidenciaId).IsUnique();

        b.HasOne(v => v.Incidencia)
         .WithOne()
         .HasForeignKey<ValoracionTicket>(v => v.IncidenciaId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(v => v.Solicitante)
         .WithMany()
         .HasForeignKey(v => v.SolicitanteId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}
