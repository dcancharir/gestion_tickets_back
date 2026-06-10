using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities;

[Table("Usuarios")]
public class Usuario {
    public int UsuarioId { get; set; }  // PK interna — NUNCA sale al frontend
    public Guid PublicId { get; set; }  // Identificador público — único expuesto
    public string Nombre { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int RolId { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    // Navegación
    public Rol Rol { get; set; } = null!;
    public ICollection<Incidencia> IncidenciasComoSolicitante { get; set; } = new List<Incidencia>();
    public ICollection<Incidencia> IncidenciasComoTecnico { get; set; } = new List<Incidencia>();
    public ICollection<Incidencia> IncidenciasEscaladas { get; set; } = new List<Incidencia>();
    public ICollection<Incidencia> IncidenciasCerradas { get; set; } = new List<Incidencia>();
    public ICollection<HistorialIncidencia> HistorialAcciones { get; set; } = new List<HistorialIncidencia>();
    public ICollection<ComentarioIncidencia> Comentarios { get; set; } = new List<ComentarioIncidencia>();
    public ICollection<BaseConocimiento> ArticulosCreados { get; set; } = new List<BaseConocimiento>();
    public ICollection<UsuarioSede> UsuarioSedes { get; set; } = new List<UsuarioSede>();
    public string UserName { get; set; } = string.Empty;
    public bool HasFullAccess { get; set; } = false;

    // ── Recuperación de contraseña ────────────────────────────────────────────
    /// <summary>Token único para restablecer contraseña. Null cuando no hay solicitud activa.</summary>
    public string? TokenRecuperacion { get; set; }
    /// <summary>Fecha de expiración del token (válido 2 horas).</summary>
    public DateTime? TokenExpiracion { get; set; }
}

