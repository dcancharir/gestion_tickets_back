using Application.CQRS.Core;
using Application.DTOS.Incidencias;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Incidencias;

// ─────────────────────────────────────────────────────────────────────────────
// Query: Obtener todas las incidencias
// ─────────────────────────────────────────────────────────────────────────────

public record ObtenerIncidenciasQuery() : IQuery<IEnumerable<IncidenciaListItemDto>>;

public class ObtenerIncidenciasHandler
    : IQueryHandler<ObtenerIncidenciasQuery, IEnumerable<IncidenciaListItemDto>> {
    private readonly IIncidenciaRepository _repo;
    public ObtenerIncidenciasHandler(IIncidenciaRepository repo) => _repo = repo;

    public async Task<IEnumerable<IncidenciaListItemDto>> HandleAsync(
        ObtenerIncidenciasQuery query, CancellationToken ct = default) {
        var incidencias = await _repo.ObtenerTodasAsync(ct);
        return incidencias.Select(IncidenciaMapper.ToListItem);
    }
}
