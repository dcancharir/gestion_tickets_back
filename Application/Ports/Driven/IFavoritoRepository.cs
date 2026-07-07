using Application.DTOS.Favoritos;

namespace Application.Ports.Driven;

public interface IFavoritoRepository {
    Task<IReadOnlyList<FavoritoDto>> ObtenerAsync(int usuarioId, CancellationToken ct = default);
    Task AgregarAsync(int usuarioId, Guid publicId, CancellationToken ct = default);
    Task EliminarAsync(int usuarioId, Guid publicId, CancellationToken ct = default);
}
