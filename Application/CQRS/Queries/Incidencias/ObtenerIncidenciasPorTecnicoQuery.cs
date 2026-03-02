using Application.CQRS.Core;
using Application.DTOS.Incidencias;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Incidencias;
// ─────────────────────────────────────────────────────────────────────────────
// Query: Incidencias asignadas a un técnico
// ─────────────────────────────────────────────────────────────────────────────

public record ObtenerIncidenciasPorTecnicoQuery(int TecnicoId) : IQuery<IEnumerable<IncidenciaListItemDto>>;

public class ObtenerIncidenciasPorTecnicoHandler
    : IQueryHandler<ObtenerIncidenciasPorTecnicoQuery, IEnumerable<IncidenciaListItemDto>> {
    private readonly IIncidenciaRepository _repo;
    public ObtenerIncidenciasPorTecnicoHandler(IIncidenciaRepository repo) => _repo = repo;

    public async Task<IEnumerable<IncidenciaListItemDto>> HandleAsync(
        ObtenerIncidenciasPorTecnicoQuery query, CancellationToken ct = default) {
        var incidencias = await _repo.ObtenerPorTecnicoAsync(query.TecnicoId, ct);
        return incidencias.Select(IncidenciaMapper.ToListItem);
    }
}
