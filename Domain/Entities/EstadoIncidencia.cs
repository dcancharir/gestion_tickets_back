using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities;

[Table("EstadosIncidencia")]
public class EstadoIncidencia {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EstadoId { get; set; }

    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    public string? Descripcion { get; set; }

    /// <summary>true si este estado representa el fin del ciclo de vida del ticket (Cerrado, Cancelado)</summary>
    public bool EsEstadoFinal { get; set; } = false;

    // Navegación
    public ICollection<Incidencia> Incidencias { get; set; } = new List<Incidencia>();
}