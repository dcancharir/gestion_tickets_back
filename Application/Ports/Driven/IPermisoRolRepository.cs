using Domain.Entities;

namespace Application.Ports.Driven;

public interface IPermisoRolRepository
{
    Task<IEnumerable<PermisoRol>> ObtenerTodosPorRolIdAsync(int rolId, CancellationToken ct=default);
    Task<bool> VerificarSiRolTienePermisoDeVistaAsync(int rolId, string uri, CancellationToken ct = default);
    Task<bool> VerificarSiRolTienePermisoDeControladorAsync(int rolId, string actionName,string controllerName, CancellationToken ct = default);
}