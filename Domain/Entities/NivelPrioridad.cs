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
    public byte Impacto { get; set; }
    public byte Urgencia { get;set; }

    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    /// <summary>1 = Crítico, 2 = Alto, 3 = Medio, 4 = Bajo</summary>
    [Required]
    [Range(1, 4)]
    public byte Nivel { get; set; }

    public string Descripcion { get; set; } = string.Empty;

    /// <summary>Minutos para dar la primera respuesta según SLA</summary>
    [Required]
    public int TiempoRespuestaMin { get; set; }

    /// <summary>Minutos para resolver la incidencia según SLA</summary>
    [Required]
    public int TiempoResolucionMin { get; set; }

    // Navegación
    public ICollection<Incidencia> Incidencias { get; set; } = new List<Incidencia>();
    public ICollection<AcuerdoNivelServicio> AcuerdosNivelServicio { get; set; } = new List<AcuerdoNivelServicio>();
    public string ColorHexa { get; set; } = string.Empty;
    public string CssClass { get; set; } = string.Empty;
    public string GuiaValor {  get; set; } = string.Empty;
}
