using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface IIncidenciaAdjuntoRepository {
    Task<IEnumerable<IncidenciaAdjunto>> ObtenerPorIncidenciaAsync(int incidenciaId, CancellationToken ct);
    Task EliminarAsync(int incidenciaAdjuntoId, CancellationToken ct);
    Task<bool> CrearRangeAsync(List<IncidenciaAdjunto> items, CancellationToken ct);
    Task<IncidenciaAdjunto> CrearAsync(IncidenciaAdjunto item, CancellationToken ct);
}
