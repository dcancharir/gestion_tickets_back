using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[Table("Permisos")]
public class Permiso
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PermisoId { get; set; }
    
    [Required]
    [StringLength(250)]
    public string Nombre { get; set; }
    
    [Required]
    [StringLength(250)]
    public string Tipo { get; set; }
    
    [Required]
    [StringLength(250)]
    public string Controlador { get; set; }
    // Navegacion
    public ICollection<PermisoRol> PermisoRoles { get; set; } = new List<PermisoRol>();
}