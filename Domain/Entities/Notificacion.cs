using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Notificaciones")]
public class Notificacion {
    public int NotificacionId { get; set; }
    public int UsuarioId { get; set; }

    public string  Tipo       { get; set; } = null!;
    public string? Referencia { get; set; }   // identificador visible: "TK-001", "ART-007", null para notificaciones de sistema
    public string  Titulo     { get; set; } = null!;
    public string  Mensaje    { get; set; } = null!;
    public string? UrlDestino { get; set; }   // ruta Angular destino del click; null = sin redirección

    public bool Leida { get; set; } = false;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public DateTime? FechaLectura { get; set; }

    // Navegación
    public Usuario Usuario { get; set; } = null!;
}
