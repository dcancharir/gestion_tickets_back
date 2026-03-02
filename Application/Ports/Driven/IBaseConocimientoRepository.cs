using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface IBaseConocimientoRepository {
    Task<IEnumerable<BaseConocimiento>> ObtenerTodosAsync(bool soloActivos = true, CancellationToken ct = default);
    Task<BaseConocimiento?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<BaseConocimiento?> ObtenerPorPublicIdAsync(Guid publicId, CancellationToken ct = default);
    Task<IEnumerable<BaseConocimiento>> BuscarAsync(string termino, CancellationToken ct = default);
    Task<IEnumerable<BaseConocimiento>> ObtenerPorCategoriaAsync(int categoriaId, CancellationToken ct = default);
    Task<BaseConocimiento> CrearAsync(BaseConocimiento articulo, CancellationToken ct = default);
    Task<BaseConocimiento> ActualizarAsync(BaseConocimiento articulo, CancellationToken ct = default);
    Task EliminarAsync(int id, CancellationToken ct = default);
}
