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
// ASIGNAR INCIDENCIA
// RolId = 1 (Admin)   → puede asignar a cualquier técnico
// RolId = 2 (Técnico) → solo puede auto-asignarse a sí mismo
// ─────────────────────────────────────────────────────────────────────────────

public record AsignarIncidenciaCommand(
    Guid PublicId,
    Guid TecnicoPublicId,
    int UsuarioId,   // quien ejecuta, viene del JWT
    int RolId        // rol de quien ejecuta, viene del JWT
) : ICommand<IncidenciaListItemDto>;

public class AsignarIncidenciaHandler
    : ICommandHandler<AsignarIncidenciaCommand, IncidenciaListItemDto> {
    private readonly IIncidenciaRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;

    public AsignarIncidenciaHandler(
        IIncidenciaRepository repo,
        IUsuarioRepository usuarioRepo) {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
    }

    public async Task<IncidenciaListItemDto> HandleAsync(
        AsignarIncidenciaCommand cmd, CancellationToken ct = default) {
        var incidencia = await _repo.ObtenerPorPublicIdAsync(cmd.PublicId, ct)
            ?? throw new NotFoundException(nameof(Incidencia), cmd.PublicId);

        if(incidencia.EstadoIncidencia.EsEstadoFinal)
            throw new ValidationException(
                $"La incidencia {incidencia.NumeroTicket} ya está cerrada.");

        var tecnico = await _usuarioRepo.ObtenerPorPublicIdAsync(cmd.TecnicoPublicId, ct)
            ?? throw new NotFoundException("Técnico", cmd.TecnicoPublicId);

        // Un técnico solo puede auto-asignarse, no asignar a otro
        if(cmd.RolId == 2 && tecnico.UsuarioId != cmd.UsuarioId)
            throw new UnauthorizedException(
                "Un técnico solo puede asignarse tickets a sí mismo.");

        var esReasignacion = incidencia.TecnicoAsignadoId.HasValue;
        var ahora = DateTime.Now;

        incidencia.TecnicoAsignadoId = tecnico.UsuarioId;
        incidencia.FechaAsignacion = ahora;
        incidencia.EstadoId = 2; // Asignado
        incidencia.FechaUltimaActualizacion = ahora;

        if(esReasignacion)
            incidencia.NumeroReasignaciones++;

        await _repo.ActualizarAsync(incidencia, ct);

        var accion = esReasignacion ? "Reasignación" : "Asignación";
        var detalle = cmd.RolId == 2
            ? $"Técnico {tecnico.Nombre} {tecnico.Apellidos} se auto-asignó el ticket"
            : $"Asignado por administrador a {tecnico.Nombre} {tecnico.Apellidos}";

        await _repo.AgregarHistorialAsync(new HistorialIncidencia {
            IncidenciaId = incidencia.IncidenciaId,
            UsuarioId = cmd.UsuarioId,
            Accion = accion,
            EstadoAnterior = incidencia.EstadoIncidencia.Nombre,
            EstadoNuevo = "Asignado",
            Detalle = detalle
        }, ct);

        var actualizada = await _repo.ObtenerPorIdAsync(incidencia.IncidenciaId, ct);
        return IncidenciaMapper.ToListItem(actualizada!);
    }
}
