using Application.CQRS.Core;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.SLAs;

public record EliminarSlaCommand(int SlaId) : ICommand;

public class EliminarSlaHandler : ICommandHandler<EliminarSlaCommand> {
    private readonly IAcuerdoNivelServicioRepository _repo;
    public EliminarSlaHandler(IAcuerdoNivelServicioRepository repo) => _repo = repo;

    public async Task<Unit> HandleAsync(EliminarSlaCommand cmd, CancellationToken ct = default) {
        _ = await _repo.ObtenerPorIdAsync(cmd.SlaId, ct)
            ?? throw new NotFoundException(nameof(AcuerdoNivelServicio), cmd.SlaId);
        await _repo.EliminarAsync(cmd.SlaId, ct);
        return Unit.Value;
    }
}
