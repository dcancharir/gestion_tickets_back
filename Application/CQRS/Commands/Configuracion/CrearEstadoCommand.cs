using Application.CQRS.Core;
using Application.DTOS.Configuracion;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Configuracion;

public record CrearEstadoCommand(string Nombre, string? Descripcion, bool EsEstadoFinal) : ICommand<EstadoIncidenciaDto>;

public class CrearEstadoHandler : ICommandHandler<CrearEstadoCommand, EstadoIncidenciaDto> {
    private readonly IEstadoIncidenciaRepository _repo;
    public CrearEstadoHandler(IEstadoIncidenciaRepository repo) => _repo = repo;

    public async Task<EstadoIncidenciaDto> HandleAsync(CrearEstadoCommand cmd, CancellationToken ct = default) {
        if(await _repo.ExisteNombreAsync(cmd.Nombre, ct: ct))
            throw new ConflictException($"El estado '{cmd.Nombre}' ya existe.");

        var estado = new EstadoIncidencia { Nombre = cmd.Nombre, Descripcion = cmd.Descripcion, EsEstadoFinal = cmd.EsEstadoFinal };
        var creado = await _repo.CrearAsync(estado, ct);
        return new EstadoIncidenciaDto(creado.EstadoId, creado.Nombre, creado.Descripcion, creado.EsEstadoFinal);
    }
}