using Application.CQRS.Core;
using Application.DTOS.Usuarios;
using Application.Ports.Driven;

namespace Application.CQRS.Queries.UsuarioSede;

// ── Query ─────────────────────────────────────────────────────────────────────

public record ObtenerUsuariosPorSedeQuery(int SedeId) : IQuery<IEnumerable<UsuarioDto>>;

// ── Handler ───────────────────────────────────────────────────────────────────

public class ObtenerUsuariosPorSedeHandler
    : IQueryHandler<ObtenerUsuariosPorSedeQuery, IEnumerable<UsuarioDto>> {
    private readonly IUsuarioSedeRepository _usuarioSedeRepo;

    public ObtenerUsuariosPorSedeHandler(IUsuarioSedeRepository usuarioSedeRepo) {
        _usuarioSedeRepo = usuarioSedeRepo;
    }

    public async Task<IEnumerable<UsuarioDto>> HandleAsync(
        ObtenerUsuariosPorSedeQuery query,
        CancellationToken ct = default) {
        var usuarios = await _usuarioSedeRepo.ObtenerUsuariosPorSedeIdAsync(query.SedeId, ct);

        return usuarios.Select(u => new UsuarioDto(
            u.PublicId,
            u.Nombre,
            u.Apellidos,
            u.Email,
            u.RolId,
            u.Rol!.Nombre,
            u.Activo,
            u.FechaCreacion,
            u.UserName
        ));
    }
}
