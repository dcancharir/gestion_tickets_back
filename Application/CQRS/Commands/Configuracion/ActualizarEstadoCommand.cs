using Application.CQRS.Core;
using Application.DTOS.Configuracion;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Configuracion;

public record ActualizarEstadoCommand(int EstadoId, string Nombre, string? Descripcion, bool EsEstadoFinal) : ICommand<EstadoIncidenciaDto>;

public class ActualizarEstadoHandler : ICommandHandler<ActualizarEstadoCommand, EstadoIncidenciaDto> {
    private readonly IEstadoIncidenciaRepository _repo;
    public ActualizarEstadoHandler(IEstadoIncidenciaRepository repo) => _repo = repo;

    public async Task<EstadoIncidenciaDto> HandleAsync(ActualizarEstadoCommand cmd, CancellationToken ct = default) {
        var estado = await _repo.ObtenerPorIdAsync(cmd.EstadoId, ct)
            ?? throw new NotFoundException(nameof(EstadoIncidencia), cmd.EstadoId);

        if(await _repo.ExisteNombreAsync(cmd.Nombre, excluirId: cmd.EstadoId, ct: ct))
            throw new ConflictException($"El nombre '{cmd.Nombre}' ya está en uso.");

        estado.Nombre = cmd.Nombre;
        estado.Descripcion = cmd.Descripcion;
        estado.EsEstadoFinal = cmd.EsEstadoFinal;

        var actualizado = await _repo.ActualizarAsync(estado, ct);
        return new EstadoIncidenciaDto(actualizado.EstadoId, actualizado.Nombre, actualizado.Descripcion, actualizado.EsEstadoFinal);
    }
}
