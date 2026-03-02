using Application.CQRS.Core;
using Application.DTOS.SLAs;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.SLAs;

public record CrearSlaCommand(int CategoriaId, int PrioridadId, int TiempoRespuestaMin, int TiempoResolucionMin) : ICommand<SlaDto>;

public class CrearSlaHandler : ICommandHandler<CrearSlaCommand, SlaDto> {
    private readonly IAcuerdoNivelServicioRepository _repo;
    public CrearSlaHandler(IAcuerdoNivelServicioRepository repo) => _repo = repo;

    public async Task<SlaDto> HandleAsync(CrearSlaCommand cmd, CancellationToken ct = default) {
        if(await _repo.ExisteCombinacionAsync(cmd.CategoriaId, cmd.PrioridadId, ct: ct))
            throw new ConflictException("Ya existe un SLA para esa combinación de Categoría y Prioridad.");

        var sla = new AcuerdoNivelServicio {
            CategoriaId = cmd.CategoriaId,
            PrioridadId = cmd.PrioridadId,
            TiempoRespuestaMin = cmd.TiempoRespuestaMin,
            TiempoResolucionMin = cmd.TiempoResolucionMin,
            Activo = true
        };

        var creado = await _repo.CrearAsync(sla, ct);
        var conNavegacion = await _repo.ObtenerPorIdAsync(creado.SlaId, ct);
        return new SlaDto(conNavegacion!.SlaId, conNavegacion.CategoriaId, conNavegacion.Categoria.Nombre,
            conNavegacion.PrioridadId, conNavegacion.NivelPrioridad.Nombre,
            conNavegacion.TiempoRespuestaMin, conNavegacion.TiempoResolucionMin, conNavegacion.Activo);
    }
}

