using Application.CQRS.Core;
using Application.DTOS.Favoritos;
using Application.Ports.Driven;

namespace Application.CQRS.Queries.Favoritos;

public record ObtenerFavoritosQuery(int UsuarioId) : IQuery<IReadOnlyList<FavoritoDto>>;

public class ObtenerFavoritosHandler : IQueryHandler<ObtenerFavoritosQuery, IReadOnlyList<FavoritoDto>> {
    private readonly IFavoritoRepository _repo;
    public ObtenerFavoritosHandler(IFavoritoRepository repo) => _repo = repo;

    public Task<IReadOnlyList<FavoritoDto>> HandleAsync(ObtenerFavoritosQuery q, CancellationToken ct = default)
        => _repo.ObtenerAsync(q.UsuarioId, ct);
}
