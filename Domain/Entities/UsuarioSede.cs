using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities;

[Table("UsuarioSede")]
public class UsuarioSede {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UsuarioSedeId { get; set; }
    
    public int UsuarioId { get; set; }
    public int SedeId { get; set; }
    [ForeignKey(nameof(UsuarioId))]
    public Usuario Usuario { get; set; } = null!;
    [ForeignKey(nameof(SedeId))]
    public Sede Sede { get; set; } = null!;
}
