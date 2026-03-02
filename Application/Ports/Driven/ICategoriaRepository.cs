using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface ICategoriaRepository {
    Task<IEnumerable<Categoria>> ObtenerTodasAsync(CancellationToken ct = default);
    Task<IEnumerable<Categoria>> ObtenerActivasAsync(CancellationToken ct = default);
    Task<Categoria?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null, CancellationToken ct = default);
    Task<Categoria> CrearAsync(Categoria categoria, CancellationToken ct = default);
    Task<Categoria> ActualizarAsync(Categoria categoria, CancellationToken ct = default);
    Task EliminarAsync(int id, CancellationToken ct = default);
}
