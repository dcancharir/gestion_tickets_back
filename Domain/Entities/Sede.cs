using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("Sedes")]
public class Sede
{
    public int SedeId { get; set; }
    public int SedeIdExterno {  get; set; }
    public string Nombre { get; set; } = null!;
    public int TipoSede { get; set; }
    public ICollection<Incidencia> Incidencias { get; set; } = new List<Incidencia>();
}