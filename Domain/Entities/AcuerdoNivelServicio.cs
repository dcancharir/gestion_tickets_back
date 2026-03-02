using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities;

[Table("AcuerdosNivelServicio")]
public class AcuerdoNivelServicio {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SlaId { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    [Required]
    public int PrioridadId { get; set; }

    [Required]
    public int TiempoRespuestaMin { get; set; }

    [Required]
    public int TiempoResolucionMin { get; set; }

    public bool Activo { get; set; } = true;

    // Navegación
    [ForeignKey(nameof(CategoriaId))]
    public Categoria Categoria { get; set; } = null!;

    [ForeignKey(nameof(PrioridadId))]
    public NivelPrioridad NivelPrioridad { get; set; } = null!;
}