using Domain.Entities;

namespace Application.Ports.Driven;

public interface IPermisoRolRepository
{
    Task<PermisoRol> CrearAsync(PermisoRol permisoRol, CancellationToken ct = default);
    Task<bool> CrearRangoAsync(List<PermisoRol> permisosRol, CancellationToken ct = default);
    Task<bool> EliminarAsync(int id, CancellationToken ct = default);
    Task<bool> EliminarRangoAsync(List<int> ids, CancellationToken ct = default);
    Task<IEnumerable<PermisoRol>> ObtenerTodosPorRolIdAsync(int rolId, CancellationToken ct=default);
    Task<bool> VerificarSiRolTienePermisoDeVistaAsync(int rolId, string uri, CancellationToken ct = default);
    Task<bool> VerificarSiRolTienePermisoDeControladorAsync(int rolId, string actionName,string controllerName, CancellationToken ct = default);
    Task<PermisoRol?> ObtenerPorPermisoYRol(int PermisoId, int RolId);
}