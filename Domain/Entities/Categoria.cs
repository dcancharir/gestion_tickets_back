using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities;

[Table("Categorias")]
public class Categoria {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;

    // Navegación
    public ICollection<Incidencia> Incidencias { get; set; } = new List<Incidencia>();
    public ICollection<AcuerdoNivelServicio> AcuerdosNivelServicio { get; set; } = new List<AcuerdoNivelServicio>();
    public ICollection<BaseConocimiento> ArticulosConocimiento { get; set; } = new List<BaseConocimiento>();
}
