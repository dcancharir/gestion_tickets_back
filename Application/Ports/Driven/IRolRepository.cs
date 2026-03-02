using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface IRolRepository {
    Task<IEnumerable<Rol>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Rol?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null, CancellationToken ct = default);
    Task<Rol> CrearAsync(Rol rol, CancellationToken ct = default);
    Task<Rol> ActualizarAsync(Rol rol, CancellationToken ct = default);
    Task EliminarAsync(int id, CancellationToken ct = default);
}