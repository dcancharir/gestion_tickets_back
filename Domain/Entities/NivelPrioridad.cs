using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities;

[Table("NivelesPrioridad")]
public class NivelPrioridad {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PrioridadId { get; set; }

    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    /// <summary>1 = Crítico, 2 = Alto, 3 = Medio, 4 = Bajo, 5 = Planificado</summary>
    [Required]
    [Range(1, 5)]
    public byte Nivel { get; set; }

    /// <summary>Minutos para dar la primera respuesta según SLA</summary>
    [Required]
    public int TiempoRespuestaMin { get; set; }

    /// <summary>Minutos para resolver la incidencia según SLA</summary>
    [Required]
    public int TiempoResolucionMin { get; set; }

    // Navegación
    public ICollection<Incidencia> Incidencias { get; set; } = new List<Incidencia>();
    public ICollection<AcuerdoNivelServicio> AcuerdosNivelServicio { get; set; } = new List<AcuerdoNivelServicio>();
}
