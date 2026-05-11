using Application.CQRS.Core;
using Application.DTOS.Sedes;
using Application.Exceptions;
using Application.Ports.Driven;

namespace Application.CQRS.Queries.UsuarioSede;

public record ObtenerSedesPorUsuarioQuery(Guid PublicId) : IQuery<IEnumerable<SedeDto>>;

public class ObtenerSedesPorUsuarioHandler : IQueryHandler<ObtenerSedesPorUsuarioQuery, IEnumerable<SedeDto>> {
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUsuarioSedeRepository _usuarioSedeRepo;

    public ObtenerSedesPorUsuarioHandler(IUsuarioRepository usuarioRepo, IUsuarioSedeRepository usuarioSedeRepo) {
        _usuarioRepo = usuarioRepo;
        _usuarioSedeRepo = usuarioSedeRepo;
    }

    public async Task<IEnumerable<SedeDto>> HandleAsync(ObtenerSedesPorUsuarioQuery query, CancellationToken ct = default) {
        var usuario = await _usuarioRepo.ObtenerPorPublicIdAsync(query.PublicId, ct)
            ?? throw new NotFoundException("Usuario", query.PublicId);

        var usuarioSedes = await _usuarioSedeRepo.ObtenerPorUsuarioIdAsync(usuario.UsuarioId, ct);
        return usuarioSedes.Select(us => new SedeDto(us.Sede.SedeId, us.Sede.SedeIdExterno, us.Sede.Nombre, us.Sede.TipoSede));
    }
}
