using Application.CQRS.Core;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Configuracion;

public record EliminarNivelPrioridadCommand(int PrioridadId) : ICommand;

public class EliminarNivelPrioridadHandler : ICommandHandler<EliminarNivelPrioridadCommand> {
    private readonly INivelPrioridadRepository _repo;
    public EliminarNivelPrioridadHandler(INivelPrioridadRepository repo) => _repo = repo;

    public async Task<Unit> HandleAsync(EliminarNivelPrioridadCommand cmd, CancellationToken ct = default) {
        _ = await _repo.ObtenerPorIdAsync(cmd.PrioridadId, ct)
            ?? throw new NotFoundException(nameof(NivelPrioridad), cmd.PrioridadId);
        await _repo.EliminarAsync(cmd.PrioridadId, ct);
        return Unit.Value;
    }
}