using Application.CQRS.Core;
using Application.DTOS.Incidencias;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Incidencias;

// ─────────────────────────────────────────────────────────────────────────────
// Query: Obtener detalle de una incidencia por PublicId
// ─────────────────────────────────────────────────────────────────────────────

public record ObtenerIncidenciaPorPublicIdQuery(Guid PublicId) : IQuery<IncidenciaDetalleDto>;

public class ObtenerIncidenciaPorPublicIdHandler
    : IQueryHandler<ObtenerIncidenciaPorPublicIdQuery, IncidenciaDetalleDto> {
    private readonly IIncidenciaRepository _repo;
    public ObtenerIncidenciaPorPublicIdHandler(IIncidenciaRepository repo) => _repo = repo;

    public async Task<IncidenciaDetalleDto> HandleAsync(
        ObtenerIncidenciaPorPublicIdQuery query, CancellationToken ct = default) {
        var incidencia = await _repo.ObtenerPorPublicIdAsync(query.PublicId, ct)
            ?? throw new NotFoundException(nameof(Incidencia), query.PublicId);

        var historial = await _repo.ObtenerHistorialAsync(incidencia.IncidenciaId, ct);
        var comentarios = await _repo.ObtenerComentariosAsync(incidencia.IncidenciaId, ct);

        return IncidenciaMapper.ToDetalle(incidencia, historial, comentarios);
    }
}
