using Application.CQRS.Core;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Categorias;

public record EliminarCategoriaCommand(int CategoriaId) : ICommand;

public class EliminarCategoriaHandler : ICommandHandler<EliminarCategoriaCommand> {
    private readonly ICategoriaRepository _repo;
    public EliminarCategoriaHandler(ICategoriaRepository repo) => _repo = repo;

    public async Task<Unit> HandleAsync(EliminarCategoriaCommand cmd, CancellationToken ct = default) {
        _ = await _repo.ObtenerPorIdAsync(cmd.CategoriaId, ct)
            ?? throw new NotFoundException(nameof(Categoria), cmd.CategoriaId);
        await _repo.EliminarAsync(cmd.CategoriaId, ct);
        return Unit.Value;
    }
}
