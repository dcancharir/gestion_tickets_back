using Application.CQRS.Core;
using Application.DTOS.BaseConocimiento;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.BaseConocimientos;

public record ObtenerArticulosPorCategoriaQuery(int CategoriaId)
    : IQuery<IEnumerable<ArticuloListItemDto>>;

public class ObtenerArticulosPorCategoriaHandler
    : IQueryHandler<ObtenerArticulosPorCategoriaQuery, IEnumerable<ArticuloListItemDto>> {
    private readonly IBaseConocimientoRepository _repo;
    public ObtenerArticulosPorCategoriaHandler(IBaseConocimientoRepository repo) => _repo = repo;

    public async Task<IEnumerable<ArticuloListItemDto>> HandleAsync(
        ObtenerArticulosPorCategoriaQuery q, CancellationToken ct = default) {
        var articulos = await _repo.ObtenerPorCategoriaAsync(q.CategoriaId, ct);
        return articulos.Select(ArticuloMapper.ToListItem);
    }
}
