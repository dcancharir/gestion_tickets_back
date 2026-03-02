using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface INivelPrioridadRepository {
    Task<IEnumerable<NivelPrioridad>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<NivelPrioridad?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null, CancellationToken ct = default);
    Task<NivelPrioridad> CrearAsync(NivelPrioridad nivel, CancellationToken ct = default);
    Task<NivelPrioridad> ActualizarAsync(NivelPrioridad nivel, CancellationToken ct = default);
    Task EliminarAsync(int id, CancellationToken ct = default);
}
