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
// 7. REABRIR INCIDENCIA
// El solicitante reporta que el problema persiste tras el cierre.
// ─────────────────────────────────────────────────────────────────────────────

public record ReabrirIncidenciaCommand(
    Guid PublicId,
    string Motivo,
    int SolicitanteId   // viene del JWT
) : ICommand<IncidenciaListItemDto>;

public class ReabrirIncidenciaHandler
    : ICommandHandler<ReabrirIncidenciaCommand, IncidenciaListItemDto> {
    private readonly IIncidenciaRepository _repo;

    public ReabrirIncidenciaHandler(IIncidenciaRepository repo) => _repo = repo;

    public async Task<IncidenciaListItemDto> HandleAsync(
        ReabrirIncidenciaCommand cmd, CancellationToken ct = default) {
        var incidencia = await _repo.ObtenerPorPublicIdAsync(cmd.PublicId, ct)
            ?? throw new NotFoundException(nameof(Incidencia), cmd.PublicId);

        // Solo se puede reabrir si está en estado Resuelto o Cerrado
        if(incidencia.EstadoId is not (6 or 7))
            throw new ValidationException(
                "Solo se puede reabrir una incidencia en estado 'Resuelto' o 'Cerrado'.");

        if(incidencia.SolicitanteId != cmd.SolicitanteId)
            throw new UnauthorizedException(
                "No tienes permiso para reabrir esta incidencia.");

        var ahora = DateTime.Now;

        incidencia.EstadoId = 8; // Reabierto
        incidencia.FechaResolucion = null;
        incidencia.FechaCierre = null;
        incidencia.SolucionAplicada = null;
        incidencia.FechaUltimaActualizacion = ahora;

        await _repo.ActualizarAsync(incidencia, ct);

        await _repo.AgregarHistorialAsync(new HistorialIncidencia {
            IncidenciaId = incidencia.IncidenciaId,
            UsuarioId = cmd.SolicitanteId,
            Accion = "Reapertura",
            EstadoAnterior = incidencia.EstadoIncidencia.Nombre,
            EstadoNuevo = "Reabierto",
            Detalle = cmd.Motivo
        }, ct);

        var actualizada = await _repo.ObtenerPorIdAsync(incidencia.IncidenciaId, ct);
        return IncidenciaMapper.ToListItem(actualizada!);
    }
}
