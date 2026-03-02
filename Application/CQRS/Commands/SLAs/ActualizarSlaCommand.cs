using Application.CQRS.Core;
using Application.DTOS.SLAs;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.SLAs;

public record ActualizarSlaCommand(int SlaId, int CategoriaId, int PrioridadId, int TiempoRespuestaMin, int TiempoResolucionMin, bool Activo) : ICommand<SlaDto>;

public class ActualizarSlaHandler : ICommandHandler<ActualizarSlaCommand, SlaDto> {
    private readonly IAcuerdoNivelServicioRepository _repo;
    public ActualizarSlaHandler(IAcuerdoNivelServicioRepository repo) => _repo = repo;

    public async Task<SlaDto> HandleAsync(ActualizarSlaCommand cmd, CancellationToken ct = default) {
        var sla = await _repo.ObtenerPorIdAsync(cmd.SlaId, ct)
            ?? throw new NotFoundException(nameof(AcuerdoNivelServicio), cmd.SlaId);

        if(await _repo.ExisteCombinacionAsync(cmd.CategoriaId, cmd.PrioridadId, excluirId: cmd.SlaId, ct: ct))
            throw new ConflictException("Ya existe un SLA para esa combinación de Categoría y Prioridad.");

        sla.CategoriaId = cmd.CategoriaId;
        sla.PrioridadId = cmd.PrioridadId;
        sla.TiempoRespuestaMin = cmd.TiempoRespuestaMin;
        sla.TiempoResolucionMin = cmd.TiempoResolucionMin;
        sla.Activo = cmd.Activo;

        await _repo.ActualizarAsync(sla, ct);
        var actualizado = await _repo.ObtenerPorIdAsync(cmd.SlaId, ct);
        return new SlaDto(actualizado!.SlaId, actualizado.CategoriaId, actualizado.Categoria.Nombre,
            actualizado.PrioridadId, actualizado.NivelPrioridad.Nombre,
            actualizado.TiempoRespuestaMin, actualizado.TiempoResolucionMin, actualizado.Activo);
    }
}

