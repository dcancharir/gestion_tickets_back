using Application.CQRS.Core;
using Application.DTOS.Sedes;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Sedes;


public class ObtenerSedesQuery : IQuery<IEnumerable<SedeDto>>;
public class ObtenerSedesHandler : IQueryHandler<ObtenerSedesQuery, IEnumerable<SedeDto>> {
    private readonly ISedeRepository _repository;
    public ObtenerSedesHandler(ISedeRepository repository) => _repository = repository;
    public async Task<IEnumerable<SedeDto>> HandleAsync(ObtenerSedesQuery query, CancellationToken cancellationToken = default) {
        var sedes = await _repository.ObtenerTodasAsync(cancellationToken);
        return sedes.Select(x =>new SedeDto(SedeId: x.SedeId,SedeIdExterno: x.SedeIdExterno,Nombre: x.Nombre,TipoSede: x.TipoSede));
    }
}
