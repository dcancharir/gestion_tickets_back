using Application.CQRS.Core;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.BaseConocimientos;

public record EliminarArticuloCommand(Guid PublicId) : ICommand;

public class EliminarArticuloHandler : ICommandHandler<EliminarArticuloCommand> {
    private readonly IBaseConocimientoRepository _repo;
    public EliminarArticuloHandler(IBaseConocimientoRepository repo) => _repo = repo;

    public async Task<Unit> HandleAsync(
        EliminarArticuloCommand cmd, CancellationToken ct = default) {
        var articulo = await _repo.ObtenerPorPublicIdAsync(cmd.PublicId, ct)
            ?? throw new NotFoundException(nameof(BaseConocimiento), cmd.PublicId);

        await _repo.EliminarAsync(articulo.ArticuloId, ct);
        return Unit.Value;
    }
}

