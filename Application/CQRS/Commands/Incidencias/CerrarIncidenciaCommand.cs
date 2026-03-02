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
// CERRAR INCIDENCIA
// RolId = 1 (Admin)       → puede cerrar cualquier ticket en estado Resuelto
// RolId = 2 (Técnico)     → solo puede cerrar tickets que tenga asignados
// RolId = 3 (Solicitante) → solo puede cerrar sus propios tickets
// En todos los casos el ticket debe estar en estado Resuelto (EstadoId = 6)
// ─────────────────────────────────────────────────────────────────────────────

public record CerrarIncidenciaCommand(
    Guid PublicId,
    int UsuarioId,     // viene del JWT
    int RolId,         // viene del JWT
    string? Comentario     // opcional
) : ICommand<IncidenciaListItemDto>;

public class CerrarIncidenciaHandler
    : ICommandHandler<CerrarIncidenciaCommand, IncidenciaListItemDto> {
    private readonly IIncidenciaRepository _repo;

    public CerrarIncidenciaHandler(IIncidenciaRepository repo) => _repo = repo;

    public async Task<IncidenciaListItemDto> HandleAsync(
        CerrarIncidenciaCommand cmd, CancellationToken ct = default) {
        var incidencia = await _repo.ObtenerPorPublicIdAsync(cmd.PublicId, ct)
            ?? throw new NotFoundException(nameof(Incidencia), cmd.PublicId);

        if(incidencia.EstadoId != 6) // debe estar en Resuelto
            throw new ValidationException(
                "Solo se puede cerrar una incidencia en estado 'Resuelto'.");

        switch(cmd.RolId) {
            case 3: // Solicitante: solo sus propios tickets
                if(incidencia.SolicitanteId != cmd.UsuarioId)
                    throw new UnauthorizedException(
                        "No tienes permiso para cerrar esta incidencia.");
                break;

            case 2: // Técnico: solo los que tiene asignados
                if(incidencia.TecnicoAsignadoId != cmd.UsuarioId)
                    throw new UnauthorizedException(
                        "Solo puedes cerrar incidencias que tengas asignadas.");
                break;

                // case 1: Admin cierra cualquiera sin restricción adicional
        }

        var ahora = DateTime.Now;

        incidencia.EstadoId = 7; // Cerrado
        incidencia.FechaCierre = ahora;
        incidencia.CerradoPorId = cmd.UsuarioId;
        incidencia.FechaUltimaActualizacion = ahora;

        await _repo.ActualizarAsync(incidencia, ct);

        var detalle = cmd.RolId switch {
            3 => "Solicitante confirmó la solución como satisfactoria",
            2 => "Técnico cerró la incidencia",
            _ => "Administrador cerró la incidencia"
        };

        if(!string.IsNullOrWhiteSpace(cmd.Comentario))
            detalle += $". {cmd.Comentario}";

        await _repo.AgregarHistorialAsync(new HistorialIncidencia {
            IncidenciaId = incidencia.IncidenciaId,
            UsuarioId = cmd.UsuarioId,
            Accion = "Cierre",
            EstadoAnterior = "Resuelto",
            EstadoNuevo = "Cerrado",
            Detalle = detalle
        }, ct);

        var actualizada = await _repo.ObtenerPorIdAsync(incidencia.IncidenciaId, ct);
        return IncidenciaMapper.ToListItem(actualizada!);
    }
}
