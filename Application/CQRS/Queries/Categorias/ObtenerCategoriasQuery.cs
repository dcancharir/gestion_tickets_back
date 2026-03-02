using Application.CQRS.Core;
using Application.DTOS.Categorias;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Categorias;

public record ObtenerCategoriasQuery(bool SoloActivas = false) : IQuery<IEnumerable<CategoriaDto>>;

public class ObtenerCategoriasHandler : IQueryHandler<ObtenerCategoriasQuery, IEnumerable<CategoriaDto>> {
    private readonly ICategoriaRepository _repo;
    public ObtenerCategoriasHandler(ICategoriaRepository repo) => _repo = repo;

    public async Task<IEnumerable<CategoriaDto>> HandleAsync(ObtenerCategoriasQuery q, CancellationToken ct = default) {
        var categorias = q.SoloActivas
            ? await _repo.ObtenerActivasAsync(ct)
            : await _repo.ObtenerTodasAsync(ct);

        return categorias.Select(c => new CategoriaDto(c.CategoriaId, c.Nombre, c.Descripcion, c.Activo));
    }
}