using Application.CQRS.Core;
using Application.DTOS.Categorias;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Categorias;

public record CrearCategoriaCommand(string Nombre, string? Descripcion) : ICommand<CategoriaDto>;

public class CrearCategoriaHandler : ICommandHandler<CrearCategoriaCommand, CategoriaDto> {
    private readonly ICategoriaRepository _repo;
    public CrearCategoriaHandler(ICategoriaRepository repo) => _repo = repo;

    public async Task<CategoriaDto> HandleAsync(CrearCategoriaCommand cmd, CancellationToken ct = default) {
        if(await _repo.ExisteNombreAsync(cmd.Nombre, ct: ct))
            throw new ConflictException($"La categoría '{cmd.Nombre}' ya existe.");

        var cat = new Categoria { Nombre = cmd.Nombre, Descripcion = cmd.Descripcion, Activo = true };
        var creada = await _repo.CrearAsync(cat, ct);
        return new CategoriaDto(creada.CategoriaId, creada.Nombre, creada.Descripcion, creada.Activo);
    }
}
