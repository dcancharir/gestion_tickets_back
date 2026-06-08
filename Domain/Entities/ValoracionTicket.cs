using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("ValoracionesTicket")]
public class ValoracionTicket {
    public int  ValoracionId  { get; set; }
    public int  IncidenciaId  { get; set; }
    public int  SolicitanteId { get; set; }

    /// <summary>Puntuación de 1 (muy malo) a 5 (excelente).</summary>
    public byte Puntuacion { get; set; }

    public string? Comentario     { get; set; }
    public DateTime FechaValoracion { get; set; } = DateTime.UtcNow;

    // Navegación
    public Incidencia Incidencia  { get; set; } = null!;
    public Usuario    Solicitante { get; set; } = null!;
}
