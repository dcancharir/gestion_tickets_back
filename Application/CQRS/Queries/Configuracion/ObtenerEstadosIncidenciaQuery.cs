using Application.CQRS.Core;
using Application.DTOS.Configuracion;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Configuracion;

public record ObtenerEstadosIncidenciaQuery() : IQuery<IEnumerable<EstadoIncidenciaDto>>;

public class ObtenerEstadosIncidenciaHandler : IQueryHandler<ObtenerEstadosIncidenciaQuery, IEnumerable<EstadoIncidenciaDto>> {
    private readonly IEstadoIncidenciaRepository _repo;
    public ObtenerEstadosIncidenciaHandler(IEstadoIncidenciaRepository repo) => _repo = repo;

    public async Task<IEnumerable<EstadoIncidenciaDto>> HandleAsync(ObtenerEstadosIncidenciaQuery q, CancellationToken ct = default) {
        var estados = await _repo.ObtenerTodosAsync(ct);
        return estados.Select(e => new EstadoIncidenciaDto(e.EstadoId, e.Nombre, e.Descripcion, e.EsEstadoFinal));
    }
}