using Application.CQRS.Core;
using Application.DTOS.Usuarios;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Usuarios;

// ── Query ─────────────────────────────────────────────────────────────────────

public record ObtenerUsuariosQuery() : IQuery<IEnumerable<UsuarioDto>>;

// ── Handler ───────────────────────────────────────────────────────────────────

public class ObtenerUsuariosHandler
    : IQueryHandler<ObtenerUsuariosQuery, IEnumerable<UsuarioDto>> {
    private readonly IUsuarioRepository _repo;

    public ObtenerUsuariosHandler(IUsuarioRepository repo) {
        _repo = repo;
    }

    public async Task<IEnumerable<UsuarioDto>> HandleAsync(
        ObtenerUsuariosQuery query,
        CancellationToken ct = default) {
        var usuarios = await _repo.ObtenerTodosAsync(ct);

        return usuarios.Select(u => new UsuarioDto(
            u.PublicId,
            u.Nombre,
            u.Apellidos,
            u.Email,
            u.RolId,
            u.Rol.Nombre,
            u.Activo,
            u.FechaCreacion,
            u.UserName
        ));
    }
}