using Application.CQRS.Core;
using Application.DTOS.Incidencias;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Incidencias;

// ─────────────────────────────────────────────────────────────────────────────
// Query: Mis incidencias (solicitante ve solo las suyas)
// ─────────────────────────────────────────────────────────────────────────────

public record ObtenerMisIncidenciasQuery(int SolicitanteId) : IQuery<IEnumerable<IncidenciaListItemDto>>;

public class ObtenerMisIncidenciasHandler
    : IQueryHandler<ObtenerMisIncidenciasQuery, IEnumerable<IncidenciaListItemDto>> {
    private readonly IIncidenciaRepository _repo;
    public ObtenerMisIncidenciasHandler(IIncidenciaRepository repo) => _repo = repo;

    public async Task<IEnumerable<IncidenciaListItemDto>> HandleAsync(
        ObtenerMisIncidenciasQuery query, CancellationToken ct = default) {
        var incidencias = await _repo.ObtenerPorSolicitanteAsync(query.SolicitanteId, ct);
        return incidencias.Select(IncidenciaMapper.ToListItem);
    }
}
