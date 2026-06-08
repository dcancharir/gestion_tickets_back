using Domain.Entities;

namespace Application.Ports.Driven;

public interface IValoracionRepository {
    Task<ValoracionTicket?> ObtenerPorIncidenciaPublicIdAsync(Guid publicId, CancellationToken ct = default);
    Task<ValoracionTicket>  CrearAsync(ValoracionTicket valoracion, CancellationToken ct = default);
    Task<bool>              ExistePorIncidenciaIdAsync(int incidenciaId, CancellationToken ct = default);
}
