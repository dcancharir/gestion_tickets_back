using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("TicketsFavoritos")]
public class TicketFavorito {
    public int UsuarioId    { get; set; }
    public int IncidenciaId { get; set; }
    public DateTime FechaAgregado { get; set; } = DateTime.Now;

    // Navegación
    [ForeignKey(nameof(UsuarioId))]
    public Usuario    Usuario    { get; set; } = null!;
    [ForeignKey(nameof(IncidenciaId))]
    public Incidencia Incidencia { get; set; } = null!;
}
