using Application.CQRS.Core;
using Application.DTOS.SLAs;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.SLAs;

public record ObtenerSlaPorIdQuery(int SlaId) : IQuery<SlaDto>;

public class ObtenerSlaPorIdHandler : IQueryHandler<ObtenerSlaPorIdQuery, SlaDto> {
    private readonly IAcuerdoNivelServicioRepository _repo;
    public ObtenerSlaPorIdHandler(IAcuerdoNivelServicioRepository repo) => _repo = repo;

    public async Task<SlaDto> HandleAsync(ObtenerSlaPorIdQuery q, CancellationToken ct = default) {
        var s = await _repo.ObtenerPorIdAsync(q.SlaId, ct)
            ?? throw new NotFoundException(nameof(AcuerdoNivelServicio), q.SlaId);
        return new SlaDto(s.SlaId, s.CategoriaId, s.Categoria.Nombre, s.PrioridadId, s.NivelPrioridad.Nombre, s.TiempoRespuestaMin, s.TiempoResolucionMin, s.Activo);
    }
}
