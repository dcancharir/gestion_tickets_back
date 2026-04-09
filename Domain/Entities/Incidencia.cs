using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities;

[Table("Incidencias")]
public class Incidencia {
    public int IncidenciaId { get; set; }  // PK interna
    public Guid PublicId { get; set; }  // Identificador público

    public string NumeroTicket { get; set; } = null!;
    public string Titulo { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public int CategoriaId { get; set; }
    public string CanalReporte { get; set; } = null!;
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    public byte Impacto { get; set; }
    public byte Urgencia { get; set; }
    public int PrioridadId { get; set; }
    public int SolicitanteId { get; set; }
    public int? TecnicoAsignadoId { get; set; }
    public DateTime? FechaAsignacion { get; set; }

    public int EstadoId { get; set; }
    public DateTime? FechaLimiteRespuesta { get; set; }
    public DateTime? FechaLimiteResolucion { get; set; }
    public DateTime? FechaPrimeraRespuesta { get; set; }
    public int? EscaladoAId { get; set; }
    public DateTime? FechaEscalamiento { get; set; }
    public byte NumeroReasignaciones { get; set; } = 0;

    public string? SolucionAplicada { get; set; }
    public bool ResueltoEnPrimerContacto { get; set; } = false;
    public DateTime? FechaResolucion { get; set; }
    public DateTime? FechaCierre { get; set; }
    public int? CerradoPorId { get; set; }
    public DateTime FechaUltimaActualizacion { get; set; } = DateTime.Now;
    public int SedeId { get; set; }

    // Navegación
    public Categoria Categoria { get; set; } = null!;
    public NivelPrioridad NivelPrioridad { get; set; } = null!;
    public EstadoIncidencia EstadoIncidencia { get; set; } = null!;
    public Usuario Solicitante { get; set; } = null!;
    public Usuario? TecnicoAsignado { get; set; }
    public Usuario? EscaladoA { get; set; }
    public Usuario? CerradoPor { get; set; }
    public Sede? Sede { get; set; }
    public ICollection<HistorialIncidencia> Historial { get; set; } = new List<HistorialIncidencia>();
    public ICollection<ComentarioIncidencia> Comentarios { get; set; } = new List<ComentarioIncidencia>();
    public ICollection<IncidenciaAdjunto> Adjuntos { get; set; } = new List<IncidenciaAdjunto>();
}