using Application.CQRS.Core;
using Application.DTOS.Categorias;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Categorias;

public record ObtenerCategoriaPorIdQuery(int CategoriaId) : IQuery<CategoriaDto>;

public class ObtenerCategoriaPorIdHandler : IQueryHandler<ObtenerCategoriaPorIdQuery, CategoriaDto> {
    private readonly ICategoriaRepository _repo;
    public ObtenerCategoriaPorIdHandler(ICategoriaRepository repo) => _repo = repo;

    public async Task<CategoriaDto> HandleAsync(ObtenerCategoriaPorIdQuery q, CancellationToken ct = default) {
        var cat = await _repo.ObtenerPorIdAsync(q.CategoriaId, ct)
            ?? throw new NotFoundException(nameof(Categoria), q.CategoriaId);
        return new CategoriaDto(cat.CategoriaId, cat.Nombre, cat.Descripcion, cat.Activo);
    }
}
