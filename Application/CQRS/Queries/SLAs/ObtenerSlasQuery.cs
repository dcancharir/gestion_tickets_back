using Application.CQRS.Core;
using Application.DTOS.SLAs;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.SLAs;

public record ObtenerSlasQuery() : IQuery<IEnumerable<SlaDto>>;

public class ObtenerSlasHandler : IQueryHandler<ObtenerSlasQuery, IEnumerable<SlaDto>> {
    private readonly IAcuerdoNivelServicioRepository _repo;
    public ObtenerSlasHandler(IAcuerdoNivelServicioRepository repo) => _repo = repo;

    public async Task<IEnumerable<SlaDto>> HandleAsync(ObtenerSlasQuery q, CancellationToken ct = default) {
        var slas = await _repo.ObtenerTodosAsync(ct);
        return slas.Select(s => new SlaDto(
            s.SlaId, s.CategoriaId, s.Categoria.Nombre,
            s.PrioridadId, s.NivelPrioridad.Nombre,
            s.TiempoRespuestaMin, s.TiempoResolucionMin, s.Activo));
    }
}
