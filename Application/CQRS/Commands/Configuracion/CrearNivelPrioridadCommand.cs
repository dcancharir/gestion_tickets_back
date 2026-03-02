using Application.CQRS.Core;
using Application.DTOS.Configuracion;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Configuracion;

public record CrearNivelPrioridadCommand(string Nombre, byte Nivel, int TiempoRespuestaMin, int TiempoResolucionMin) : ICommand<NivelPrioridadDto>;

public class CrearNivelPrioridadHandler : ICommandHandler<CrearNivelPrioridadCommand, NivelPrioridadDto> {
    private readonly INivelPrioridadRepository _repo;
    public CrearNivelPrioridadHandler(INivelPrioridadRepository repo) => _repo = repo;

    public async Task<NivelPrioridadDto> HandleAsync(CrearNivelPrioridadCommand cmd, CancellationToken ct = default) {
        if(await _repo.ExisteNombreAsync(cmd.Nombre, ct: ct))
            throw new ConflictException($"El nivel de prioridad '{cmd.Nombre}' ya existe.");

        var nivel = new NivelPrioridad {
            Nombre = cmd.Nombre,
            Nivel = cmd.Nivel,
            TiempoRespuestaMin = cmd.TiempoRespuestaMin,
            TiempoResolucionMin = cmd.TiempoResolucionMin
        };
        var creado = await _repo.CrearAsync(nivel, ct);
        return new NivelPrioridadDto(creado.PrioridadId, creado.Nombre, creado.Nivel, creado.TiempoRespuestaMin, creado.TiempoResolucionMin);
    }
}