using Application.CQRS.Core;
using Application.Ports.Driven;

namespace Application.CQRS.Queries.PermisosRol;

public record VerificarAccesoVistaQuery(int UsuarioId, string Uri) : IQuery<bool>;

public class VerificarAccesoVistaQueryHandler : IQueryHandler<VerificarAccesoVistaQuery, bool>
{
    private readonly IUsuarioRepository    _usuarioRepo;
    private readonly IPermisoRolRepository _permisoRolRepo;

    public VerificarAccesoVistaQueryHandler(
        IUsuarioRepository    usuarioRepo,
        IPermisoRolRepository permisoRolRepo)
    {
        _usuarioRepo    = usuarioRepo;
        _permisoRolRepo = permisoRolRepo;
    }

    public async Task<bool> HandleAsync(VerificarAccesoVistaQuery q, CancellationToken ct = default)
    {
        var usuario = await _usuarioRepo.ObtenerPorIdAsync(q.UsuarioId, ct);

        if (usuario is null)
            return false;

        // Usuarios con acceso total omiten la verificación
        if (usuario.HasFullAccess)
            return true;

        return await _permisoRolRepo.VerificarSiRolTienePermisoDeVistaAsync(
            usuario.RolId, q.Uri, ct);
    }
}
