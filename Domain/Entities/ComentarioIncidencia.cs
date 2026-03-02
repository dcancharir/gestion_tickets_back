using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities;

[Table("ComentariosIncidencia")]
public class ComentarioIncidencia {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ComentarioId { get; set; }

    [Required]
    public int IncidenciaId { get; set; }

    [Required]
    public int UsuarioId { get; set; }

    [Required]
    public string Mensaje { get; set; } = null!;

    /// <summary>false = visible al solicitante | true = solo para técnicos</summary>
    public bool EsInterno { get; set; } = false;

    public DateTime FechaComentario { get; set; } = DateTime.Now;

    // Navegación
    [ForeignKey(nameof(IncidenciaId))]
    public Incidencia Incidencia { get; set; } = null!;

    [ForeignKey(nameof(UsuarioId))]
    public Usuario Usuario { get; set; } = null!;
}