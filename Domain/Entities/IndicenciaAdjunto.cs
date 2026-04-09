using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("IndicenciaAdjuntos")]
public class IncidenciaAdjunto
{
    public int IncidenciaAdjuntoId { get; set; }
    public int IncidenciaId { get; set; }
    public string Nombre { get; set; } = null!;
    public string RutaContenedora { get; set; } = null!;
    public string NombreReal {  get; set; } = null!;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public Incidencia Incidencia { get; set; } = null!;
}