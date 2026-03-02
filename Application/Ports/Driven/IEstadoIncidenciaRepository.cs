using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface IEstadoIncidenciaRepository {
    Task<IEnumerable<EstadoIncidencia>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<EstadoIncidencia?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null, CancellationToken ct = default);
    Task<EstadoIncidencia> CrearAsync(EstadoIncidencia estado, CancellationToken ct = default);
    Task<EstadoIncidencia> ActualizarAsync(EstadoIncidencia estado, CancellationToken ct = default);
    Task EliminarAsync(int id, CancellationToken ct = default);
}
