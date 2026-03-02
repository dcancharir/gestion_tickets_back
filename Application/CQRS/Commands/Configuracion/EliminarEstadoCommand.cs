using Application.CQRS.Core;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Configuracion;

public record EliminarEstadoCommand(int EstadoId) : ICommand;

public class EliminarEstadoHandler : ICommandHandler<EliminarEstadoCommand> {
    private readonly IEstadoIncidenciaRepository _repo;
    public EliminarEstadoHandler(IEstadoIncidenciaRepository repo) => _repo = repo;

    public async Task<Unit> HandleAsync(EliminarEstadoCommand cmd, CancellationToken ct = default) {
        _ = await _repo.ObtenerPorIdAsync(cmd.EstadoId, ct)
            ?? throw new NotFoundException(nameof(EstadoIncidencia), cmd.EstadoId);
        await _repo.EliminarAsync(cmd.EstadoId, ct);
        return Unit.Value;
    }
}
