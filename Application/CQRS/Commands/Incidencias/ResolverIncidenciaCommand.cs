using Application.CQRS.Core;
using Application.CQRS.Queries.Incidencias;
using Application.DTOS.Incidencias;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Incidencias;

// ─────────────────────────────────────────────────────────────────────────────
// 4. RESOLVER INCIDENCIA
// El técnico marca la incidencia como resuelta y documenta la solución.
// ─────────────────────────────────────────────────────────────────────────────

public record ResolverIncidenciaCommand(
    Guid PublicId,
    string SolucionAplicada,
    bool ResueltoEnPrimerContacto,
    int TecnicoId   // viene del JWT
) : ICommand<IncidenciaListItemDto>;

public class ResolverIncidenciaHandler
    : ICommandHandler<ResolverIncidenciaCommand, IncidenciaListItemDto> {
    private readonly IIncidenciaRepository _repo;

    public ResolverIncidenciaHandler(IIncidenciaRepository repo) => _repo = repo;

    public async Task<IncidenciaListItemDto> HandleAsync(
        ResolverIncidenciaCommand cmd, CancellationToken ct = default) {
        var incidencia = await _repo.ObtenerPorPublicIdAsync(cmd.PublicId, ct)
            ?? throw new NotFoundException(nameof(Incidencia), cmd.PublicId);

        if(incidencia.EstadoIncidencia.EsEstadoFinal)
            throw new ValidationException(
                $"La incidencia {incidencia.NumeroTicket} ya está cerrada.");

        var ahora = DateTime.Now;

        incidencia.EstadoId = 6; // Resuelto
        incidencia.SolucionAplicada = cmd.SolucionAplicada;
        incidencia.ResueltoEnPrimerContacto = cmd.ResueltoEnPrimerContacto;
        incidencia.FechaResolucion = ahora;
        incidencia.FechaUltimaActualizacion = ahora;

        await _repo.ActualizarAsync(incidencia, ct);

        await _repo.AgregarHistorialAsync(new HistorialIncidencia {
            IncidenciaId = incidencia.IncidenciaId,
            UsuarioId = cmd.TecnicoId,
            Accion = "Resolución",
            EstadoAnterior = incidencia.EstadoIncidencia.Nombre,
            EstadoNuevo = "Resuelto",
            Detalle = cmd.SolucionAplicada
        }, ct);

        var actualizada = await _repo.ObtenerPorIdAsync(incidencia.IncidenciaId, ct);
        return IncidenciaMapper.ToListItem(actualizada!);
    }
}
