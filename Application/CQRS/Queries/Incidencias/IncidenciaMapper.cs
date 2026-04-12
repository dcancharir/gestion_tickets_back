using Application.DTOS.Incidencias;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Incidencias;

internal static class IncidenciaMapper {
    internal static IncidenciaListItemDto ToListItem(Incidencia i) => new(
        i.PublicId,
        i.NumeroTicket,
        i.Titulo,
        i.Categoria.Nombre,
        i.NivelPrioridad.Nombre,
        i.NivelPrioridad.Nivel,
        i.EstadoIncidencia.Nombre,
        i.EstadoIncidencia.EsEstadoFinal,
        $"{i.Solicitante.Nombre} {i.Solicitante.Apellidos}",
        i.TecnicoAsignado is not null
            ? $"{i.TecnicoAsignado.Nombre} {i.TecnicoAsignado.Apellidos}"
            : null,
        i.FechaRegistro,
        i.FechaLimiteResolucion,
        i.FechaResolucion.HasValue && i.FechaLimiteResolucion.HasValue
            ? i.FechaResolucion <= i.FechaLimiteResolucion
            : null,
        i.ResueltoEnPrimerContacto,
        i.Descripcion,
        i.Sede?.Nombre is not null ? $"{i.Sede.Nombre}" : null
    );

    internal static IncidenciaDetalleDto ToDetalle(
        Incidencia i,
        IEnumerable<HistorialIncidencia> historial,
        IEnumerable<ComentarioIncidencia> comentarios) => new(
        i.PublicId,
        i.NumeroTicket,
        i.Titulo,
        i.Descripcion,
        i.Categoria.Nombre,
        i.CanalReporte,
        i.NivelPrioridad.Nombre,
        i.NivelPrioridad.Nivel,
        i.Impacto switch { 1 => "Alto", 2 => "Medio", _ => "Bajo" },
        i.Urgencia switch { 1 => "Alta", 2 => "Media", _ => "Baja" },
        i.EstadoIncidencia.Nombre,
        i.EstadoIncidencia.EsEstadoFinal,
        $"{i.Solicitante.Nombre} {i.Solicitante.Apellidos}",
        i.Solicitante.PublicId,
        i.TecnicoAsignado is not null
            ? $"{i.TecnicoAsignado.Nombre} {i.TecnicoAsignado.Apellidos}"
            : null,
        i.TecnicoAsignado?.PublicId,
        i.FechaRegistro,
        i.FechaAsignacion,
        i.FechaLimiteRespuesta,
        i.FechaLimiteResolucion,
        i.FechaPrimeraRespuesta,
        i.FechaResolucion,
        i.FechaCierre,
        i.SolucionAplicada,
        i.ResueltoEnPrimerContacto,
        i.NumeroReasignaciones,
        i.FechaResolucion.HasValue && i.FechaLimiteResolucion.HasValue
            ? i.FechaResolucion <= i.FechaLimiteResolucion
            : null,
        i.Sede?.Nombre is not null ? $"{i.Sede.Nombre}" : null,
        historial.Select(h => new HistorialDto(
            h.Accion,
            h.EstadoAnterior,
            h.EstadoNuevo,
            h.Detalle,
            $"{h.Usuario.Nombre} {h.Usuario.Apellidos}",
            h.FechaAccion
        )),
        comentarios.Select(c => new ComentarioDto(
            c.Mensaje,
            c.EsInterno,
            $"{c.Usuario.Nombre} {c.Usuario.Apellidos}",
            c.Usuario.PublicId,
            c.FechaComentario
        ))
    );
}
