using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities;

[Table("BaseConocimiento")]
public class BaseConocimiento {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ArticuloId { get; set; }
    public Guid PublicId { get; set; }
    public int? CategoriaId { get; set; }

    [Required]
    [StringLength(200)]
    public string Titulo { get; set; } = null!;

    [Required]
    public string Problema { get; set; } = null!;

    [Required]
    public string Solucion { get; set; } = null!;

    [Required]
    public int CreadoPorId { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public bool Activo { get; set; } = true;

    // Navegación
    [ForeignKey(nameof(CategoriaId))]
    public Categoria? Categoria { get; set; }

    [ForeignKey(nameof(CreadoPorId))]
    public Usuario CreadoPor { get; set; } = null!;
}