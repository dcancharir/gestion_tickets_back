using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class NotificacionConfiguration : IEntityTypeConfiguration<Notificacion> {
    public void Configure(EntityTypeBuilder<Notificacion> builder) {
        builder.ToTable("Notificaciones");
        builder.HasKey(n => n.NotificacionId);

        builder.Property(n => n.Tipo).HasMaxLength(50).IsRequired();
        builder.Property(n => n.Referencia).HasMaxLength(50).IsRequired(false);
        builder.Property(n => n.Titulo).HasMaxLength(300).IsRequired();
        builder.Property(n => n.Mensaje).HasMaxLength(500).IsRequired();
        builder.Property(n => n.UrlDestino).HasMaxLength(300).IsRequired(false);

        builder.HasOne(n => n.Usuario)
            .WithMany()
            .HasForeignKey(n => n.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
