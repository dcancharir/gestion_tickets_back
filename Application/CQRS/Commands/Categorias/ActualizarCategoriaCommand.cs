using Application.CQRS.Core;
using Application.DTOS.Categorias;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Categorias;

public record ActualizarCategoriaCommand(int CategoriaId, string Nombre, string? Descripcion, bool Activo) : ICommand<CategoriaDto>;

public class ActualizarCategoriaHandler : ICommandHandler<ActualizarCategoriaCommand, CategoriaDto> {
    private readonly ICategoriaRepository _repo;
    public ActualizarCategoriaHandler(ICategoriaRepository repo) => _repo = repo;

    public async Task<CategoriaDto> HandleAsync(ActualizarCategoriaCommand cmd, CancellationToken ct = default) {
        var cat = await _repo.ObtenerPorIdAsync(cmd.CategoriaId, ct)
            ?? throw new NotFoundException(nameof(Categoria), cmd.CategoriaId);

        if(await _repo.ExisteNombreAsync(cmd.Nombre, excluirId: cmd.CategoriaId, ct: ct))
            throw new ConflictException($"El nombre '{cmd.Nombre}' ya está en uso.");

        cat.Nombre = cmd.Nombre;
        cat.Descripcion = cmd.Descripcion;
        cat.Activo = cmd.Activo;

        var actualizada = await _repo.ActualizarAsync(cat, ct);
        return new CategoriaDto(actualizada.CategoriaId, actualizada.Nombre, actualizada.Descripcion, actualizada.Activo);
    }
}
