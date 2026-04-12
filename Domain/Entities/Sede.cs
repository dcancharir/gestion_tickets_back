using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("Sedes")]
public class Sede
{
    public int SedeId { get; set; }
    public int SedeIdExterno {  get; set; } = 0;
    public string Nombre { get; set; } = null!;
    public string TipoSede { get; set; } = null!;
    public ICollection<Incidencia> Incidencias { get; set; } = new List<Incidencia>();
}