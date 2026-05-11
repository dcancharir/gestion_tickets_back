using Domain.Entities;

namespace Application.Ports.Driven;

public interface IUsuarioSedeRepository {
    Task<IEnumerable<UsuarioSede>> ObtenerPorUsuarioIdAsync(int usuarioId, CancellationToken ct = default);
    Task<IEnumerable<Usuario>> ObtenerUsuariosPorSedeIdAsync(int sedeId, CancellationToken ct = default);
    Task AsignarSedesAsync(int usuarioId, IEnumerable<int> sedeIds, CancellationToken ct = default);
}
