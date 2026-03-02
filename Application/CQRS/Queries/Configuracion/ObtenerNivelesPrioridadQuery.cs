using Application.CQRS.Core;
using Application.DTOS.Configuracion;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Configuracion;

public record ObtenerNivelesPrioridadQuery() : IQuery<IEnumerable<NivelPrioridadDto>>;

public class ObtenerNivelesPrioridadHandler : IQueryHandler<ObtenerNivelesPrioridadQuery, IEnumerable<NivelPrioridadDto>> {
    private readonly INivelPrioridadRepository _repo;
    public ObtenerNivelesPrioridadHandler(INivelPrioridadRepository repo) => _repo = repo;

    public async Task<IEnumerable<NivelPrioridadDto>> HandleAsync(ObtenerNivelesPrioridadQuery q, CancellationToken ct = default) {
        var niveles = await _repo.ObtenerTodosAsync(ct);
        return niveles.Select(n => new NivelPrioridadDto(n.PrioridadId, n.Nombre, n.Nivel, n.TiempoRespuestaMin, n.TiempoResolucionMin));
    }
}
