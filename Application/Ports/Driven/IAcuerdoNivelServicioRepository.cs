using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface IAcuerdoNivelServicioRepository {
    Task<IEnumerable<AcuerdoNivelServicio>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<AcuerdoNivelServicio?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<AcuerdoNivelServicio?> ObtenerPorCategoriaYPrioridadAsync(int categoriaId, int prioridadId, CancellationToken ct = default);
    Task<bool> ExisteCombinacionAsync(int categoriaId, int prioridadId, int? excluirId = null, CancellationToken ct = default);
    Task<AcuerdoNivelServicio> CrearAsync(AcuerdoNivelServicio sla, CancellationToken ct = default);
    Task<AcuerdoNivelServicio> ActualizarAsync(AcuerdoNivelServicio sla, CancellationToken ct = default);
    Task EliminarAsync(int id, CancellationToken ct = default);
}