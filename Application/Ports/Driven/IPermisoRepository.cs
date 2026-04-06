using Domain.Entities;

namespace Application.Ports.Driven;

public interface IPermisoRepository
{
    Task<IEnumerable<Permiso>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Permiso> CrearAsync(Permiso permiso, CancellationToken ct = default);
    Task<bool> CrearRangoAsync(List<Permiso> permisos, CancellationToken ct = default);
    Task<bool> EliminarAsync(int id, CancellationToken ct = default);
    Task<bool> EliminarRangoAsync(List<int> ids, CancellationToken ct = default);
}