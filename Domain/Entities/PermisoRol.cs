using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("PermisosRol")]
public class PermisoRol
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PermisoRolId { get; set; }
    
    public int PermisoId { get; set; }
    public int RolId { get; set; }
    // Navegacion
    public Rol Rol { get; set; }
    public Permiso Permiso { get; set; }
}