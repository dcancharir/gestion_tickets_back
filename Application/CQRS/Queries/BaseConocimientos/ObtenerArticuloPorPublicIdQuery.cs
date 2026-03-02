using Application.CQRS.Core;
using Application.DTOS.BaseConocimiento;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.BaseConocimientos;

public record ObtenerArticuloPorPublicIdQuery(Guid PublicId) : IQuery<ArticuloDetalleDto>;

public class ObtenerArticuloPorPublicIdHandler
    : IQueryHandler<ObtenerArticuloPorPublicIdQuery, ArticuloDetalleDto> {
    private readonly IBaseConocimientoRepository _repo;
    public ObtenerArticuloPorPublicIdHandler(IBaseConocimientoRepository repo) => _repo = repo;

    public async Task<ArticuloDetalleDto> HandleAsync(
        ObtenerArticuloPorPublicIdQuery q, CancellationToken ct = default) {
        var articulo = await _repo.ObtenerPorPublicIdAsync(q.PublicId, ct)
            ?? throw new NotFoundException(nameof(BaseConocimiento), q.PublicId);
        return ArticuloMapper.ToDetalle(articulo);
    }
}