using Application.CQRS.Core;
using Application.DTOS.Usuarios;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Usuarios;

// ── Query ─────────────────────────────────────────────────────────────────────

public record ObtenerUsuarioPorPublicIdQuery(Guid PublicId) : IQuery<UsuarioDto>;

// ── Handler ───────────────────────────────────────────────────────────────────

public class ObtenerUsuarioPorPublicIdHandler
    : IQueryHandler<ObtenerUsuarioPorPublicIdQuery, UsuarioDto> {
    private readonly IUsuarioRepository _repo;

    public ObtenerUsuarioPorPublicIdHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<UsuarioDto> HandleAsync(
        ObtenerUsuarioPorPublicIdQuery query,
        CancellationToken ct = default) {
        var usuario = await _repo.ObtenerPorPublicIdAsync(query.PublicId, ct)
            ?? throw new NotFoundException(nameof(Usuario), query.PublicId);

        return ToDto(usuario);
    }

    internal static UsuarioDto ToDto(Usuario u) => new(
        u.PublicId,
        u.Nombre,
        u.Apellidos,
        u.Email,
        u.RolId,
        u.Rol.Nombre,
        u.Activo,
        u.FechaCreacion
    );
}