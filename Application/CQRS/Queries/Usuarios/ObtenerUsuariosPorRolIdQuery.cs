using Application.CQRS.Core;
using Application.DTOS.Usuarios;
using Application.Ports.Driven;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.Usuarios;

public record ObtenerUsuariosPorRolIdQuery(int rolId) : IQuery<IEnumerable<UsuarioDto>>;

public class ObtenerUsuariosPorRolIdHandler : IQueryHandler<ObtenerUsuariosPorRolIdQuery, IEnumerable<UsuarioDto>> {
    private readonly IUsuarioRepository _usuarioRepository;
    public ObtenerUsuariosPorRolIdHandler(IUsuarioRepository usuarioRepository) {
        _usuarioRepository = usuarioRepository;
    }
    public async Task<IEnumerable<UsuarioDto>> HandleAsync(ObtenerUsuariosPorRolIdQuery query, CancellationToken cancellationToken = default) {
        var usuarios = await _usuarioRepository.ObtenerPorRolId(query.rolId, cancellationToken);
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
