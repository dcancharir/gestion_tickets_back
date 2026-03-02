using Application.CQRS.Core;
using Application.DTOS.BaseConocimiento;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.BaseConocimientos;

public record ObtenerArticulosQuery(bool SoloActivos = true)
    : IQuery<IEnumerable<ArticuloListItemDto>>;

public class ObtenerArticulosHandler
    : IQueryHandler<ObtenerArticulosQuery, IEnumerable<ArticuloListItemDto>> {
    private readonly IBaseConocimientoRepository _repo;
    public ObtenerArticulosHandler(IBaseConocimientoRepository repo) => _repo = repo;

    public async Task<IEnumerable<ArticuloListItemDto>> HandleAsync(
        ObtenerArticulosQuery q, CancellationToken ct = default) {
        var articulos = await _repo.ObtenerTodosAsync(q.SoloActivos, ct);
        return articulos.Select(ArticuloMapper.ToListItem);
    }
}
