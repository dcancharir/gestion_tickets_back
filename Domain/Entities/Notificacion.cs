using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Notificaciones")]
public class Notificacion {
    public int NotificacionId { get; set; }
    public int UsuarioId { get; set; }

    public string Tipo { get; set; } = null!;           // "Asignación" | "Escalamiento" | "SLA Incumplido"
    public string TicketPublicId { get; set; } = null!;
    public string NumeroTicket { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public string Mensaje { get; set; } = null!;

    public bool Leida { get; set; } = false;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public DateTime? FechaLectura { get; set; }

    // Navegación
    public Usuario Usuario { get; set; } = null!;
}
