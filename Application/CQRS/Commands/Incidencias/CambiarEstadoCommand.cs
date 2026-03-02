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
// 3. CAMBIAR ESTADO
// El técnico cambia el estado durante la atención.
// ─────────────────────────────────────────────────────────────────────────────

public record CambiarEstadoCommand(
    Guid PublicId,
    int NuevoEstadoId,
    string Detalle,
    int UsuarioId    // quien cambia el estado, viene del JWT
) : ICommand<IncidenciaListItemDto>;

public class CambiarEstadoHandler
    : ICommandHandler<CambiarEstadoCommand, IncidenciaListItemDto> {
    private readonly IIncidenciaRepository _repo;

    public CambiarEstadoHandler(IIncidenciaRepository repo) => _repo = repo;

    public async Task<IncidenciaListItemDto> HandleAsync(
        CambiarEstadoCommand cmd, CancellationToken ct = default) {
        var incidencia = await _repo.ObtenerPorPublicIdAsync(cmd.PublicId, ct)
            ?? throw new NotFoundException(nameof(Incidencia), cmd.PublicId);

        if(incidencia.EstadoIncidencia.EsEstadoFinal)
            throw new ValidationException(
                $"La incidencia {incidencia.NumeroTicket} ya está cerrada y no puede cambiar de estado.");

        var estadoAnterior = incidencia.EstadoIncidencia.Nombre;
        var ahora = DateTime.Now;

        incidencia.EstadoId = cmd.NuevoEstadoId;
        incidencia.FechaUltimaActualizacion = ahora;

        // Registrar primera respuesta si aún no se ha registrado
        if(incidencia.FechaPrimeraRespuesta is null &&
            cmd.NuevoEstadoId is 3 or 4) // En Diagnóstico o En Progreso
        {
            incidencia.FechaPrimeraRespuesta = ahora;
        }

        await _repo.ActualizarAsync(incidencia, ct);

        await _repo.AgregarHistorialAsync(new HistorialIncidencia {
            IncidenciaId = incidencia.IncidenciaId,
            UsuarioId = cmd.UsuarioId,
            Accion = "Cambio de Estado",
            EstadoAnterior = estadoAnterior,
            EstadoNuevo = cmd.NuevoEstadoId.ToString(), // el mapper lo resolverá
            Detalle = cmd.Detalle
        }, ct);

        var actualizada = await _repo.ObtenerPorIdAsync(incidencia.IncidenciaId, ct);
        return IncidenciaMapper.ToListItem(actualizada!);
    }
}