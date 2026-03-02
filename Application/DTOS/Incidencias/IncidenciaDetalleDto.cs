using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Incidencias;

public record IncidenciaDetalleDto(
    Guid PublicId,
    string NumeroTicket,
    string Titulo,
    string Descripcion,
    string Categoria,
    string CanalReporte,
    string Prioridad,
    byte NivelPrioridad,
    string Impacto,
    string Urgencia,
    string Estado,
    bool EsEstadoFinal,
    string Solicitante,
    Guid SolicitantePublicId,
    string? Tecnico,
    Guid? TecnicoPublicId,
    DateTime FechaRegistro,
    DateTime? FechaAsignacion,
    DateTime? FechaLimiteRespuesta,
    DateTime? FechaLimiteResolucion,
    DateTime? FechaPrimeraRespuesta,
    DateTime? FechaResolucion,
    DateTime? FechaCierre,
    string? SolucionAplicada,
    bool ResueltoEnPrimerContacto,
    byte NumeroReasignaciones,
    bool? CumpleSla,
    IEnumerable<HistorialDto> Historial,
    IEnumerable<ComentarioDto> Comentarios
);

