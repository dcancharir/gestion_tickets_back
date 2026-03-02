using Application.CQRS.Core;
using Application.DTOS.BaseConocimiento;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.BaseConocimientos;

public record BuscarArticulosQuery(string Termino) : IQuery<IEnumerable<ArticuloListItemDto>>;

public class BuscarArticulosHandler
    : IQueryHandler<BuscarArticulosQuery, IEnumerable<ArticuloListItemDto>> {
    private readonly IBaseConocimientoRepository _repo;
    public BuscarArticulosHandler(IBaseConocimientoRepository repo) => _repo = repo;

    public async Task<IEnumerable<ArticuloListItemDto>> HandleAsync(
        BuscarArticulosQuery q, CancellationToken ct = default) {
        if(string.IsNullOrWhiteSpace(q.Termino))
            return Enumerable.Empty<ArticuloListItemDto>();

        var articulos = await _repo.BuscarAsync(q.Termino, ct);
        return articulos.Select(ArticuloMapper.ToListItem);
    }
}
