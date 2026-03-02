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
// 6. ESCALAR INCIDENCIA
// Admin reasigna a otro técnico con motivo documentado.
// ─────────────────────────────────────────────────────────────────────────────

public record EscalarIncidenciaCommand(
    Guid PublicId,
    Guid TecnicoPublicId,
    string Motivo,
    int AdminId   // viene del JWT
) : ICommand<IncidenciaListItemDto>;

public class EscalarIncidenciaHandler
    : ICommandHandler<EscalarIncidenciaCommand, IncidenciaListItemDto> {
    private readonly IIncidenciaRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;

    public EscalarIncidenciaHandler(
        IIncidenciaRepository repo,
        IUsuarioRepository usuarioRepo) {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
    }

    public async Task<IncidenciaListItemDto> HandleAsync(
        EscalarIncidenciaCommand cmd, CancellationToken ct = default) {
        var incidencia = await _repo.ObtenerPorPublicIdAsync(cmd.PublicId, ct)
            ?? throw new NotFoundException(nameof(Incidencia), cmd.PublicId);

        if(incidencia.EstadoIncidencia.EsEstadoFinal)
            throw new ValidationException(
                $"La incidencia {incidencia.NumeroTicket} ya está cerrada.");

        var tecnico = await _usuarioRepo.ObtenerPorPublicIdAsync(cmd.TecnicoPublicId, ct)
            ?? throw new NotFoundException("Técnico", cmd.TecnicoPublicId);

        var tecnicoAnterior = incidencia.TecnicoAsignado is not null
            ? $"{incidencia.TecnicoAsignado.Nombre} {incidencia.TecnicoAsignado.Apellidos}"
            : "Sin asignar";

        var ahora = DateTime.Now;

        incidencia.EscaladoAId = tecnico.UsuarioId;
        incidencia.FechaEscalamiento = ahora;
        incidencia.TecnicoAsignadoId = tecnico.UsuarioId;
        incidencia.FechaAsignacion = ahora;
        incidencia.NumeroReasignaciones++;
        incidencia.FechaUltimaActualizacion = ahora;

        await _repo.ActualizarAsync(incidencia, ct);

        await _repo.AgregarHistorialAsync(new HistorialIncidencia {
            IncidenciaId = incidencia.IncidenciaId,
            UsuarioId = cmd.AdminId,
            Accion = "Escalamiento",
            EstadoAnterior = incidencia.EstadoIncidencia.Nombre,
            EstadoNuevo = incidencia.EstadoIncidencia.Nombre,
            Detalle = $"De: {tecnicoAnterior} → A: {tecnico.Nombre} {tecnico.Apellidos}. Motivo: {cmd.Motivo}"
        }, ct);

        var actualizada = await _repo.ObtenerPorIdAsync(incidencia.IncidenciaId, ct);
        return IncidenciaMapper.ToListItem(actualizada!);
    }
}
