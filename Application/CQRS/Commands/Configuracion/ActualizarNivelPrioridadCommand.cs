using Application.CQRS.Core;
using Application.DTOS.Configuracion;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Configuracion;

public record ActualizarNivelPrioridadCommand(int PrioridadId, string Nombre, byte Nivel, int TiempoRespuestaMin, int TiempoResolucionMin) : ICommand<NivelPrioridadDto>;

public class ActualizarNivelPrioridadHandler : ICommandHandler<ActualizarNivelPrioridadCommand, NivelPrioridadDto> {
    private readonly INivelPrioridadRepository _repo;
    public ActualizarNivelPrioridadHandler(INivelPrioridadRepository repo) => _repo = repo;

    public async Task<NivelPrioridadDto> HandleAsync(ActualizarNivelPrioridadCommand cmd, CancellationToken ct = default) {
        var nivel = await _repo.ObtenerPorIdAsync(cmd.PrioridadId, ct)
            ?? throw new NotFoundException(nameof(NivelPrioridad), cmd.PrioridadId);

        if(await _repo.ExisteNombreAsync(cmd.Nombre, excluirId: cmd.PrioridadId, ct: ct))
            throw new ConflictException($"El nombre '{cmd.Nombre}' ya está en uso.");

        nivel.Nombre = cmd.Nombre;
        nivel.Nivel = cmd.Nivel;
        nivel.TiempoRespuestaMin = cmd.TiempoRespuestaMin;
        nivel.TiempoResolucionMin = cmd.TiempoResolucionMin;

        var actualizado = await _repo.ActualizarAsync(nivel, ct);
        return new NivelPrioridadDto(actualizado.PrioridadId, actualizado.Nombre, actualizado.Nivel, actualizado.TiempoRespuestaMin, actualizado.TiempoResolucionMin);
    }
}
