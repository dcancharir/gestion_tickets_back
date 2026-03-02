using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities;

[Table("HistorialIncidencias")]
public class HistorialIncidencia {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int HistorialId { get; set; }

    [Required]
    public int IncidenciaId { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    /// <summary>Ej: 'Registro', 'Cambio de Estado', 'Asignación', 'Escalamiento', 'Resolución', 'Cierre'</summary>
    [Required]
    [StringLength(100)]
    public string Accion { get; set; } = null!;

    [StringLength(50)]
    public string? EstadoAnterior { get; set; }

    [StringLength(50)]
    public string? EstadoNuevo { get; set; }

    public string? Detalle { get; set; }

    public DateTime FechaAccion { get; set; } = DateTime.Now;

    // Navegación
    [ForeignKey(nameof(IncidenciaId))]
    public Incidencia Incidencia { get; set; } = null!;

    [ForeignKey(nameof(UsuarioId))]
    public Usuario Usuario { get; set; } = null!;
}