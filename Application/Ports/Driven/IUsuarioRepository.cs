using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface IUsuarioRepository {
    Task<IEnumerable<Usuario>> ObtenerTodosAsync(CancellationToken ct = default);

    // Búsqueda interna (para joins y lógica de negocio)
    Task<Usuario?> ObtenerPorIdAsync(int id, CancellationToken ct = default);

    // Búsqueda pública (para endpoints expuestos al frontend)
    Task<Usuario?> ObtenerPorPublicIdAsync(Guid publicId, CancellationToken ct = default);

    Task<Usuario?> ObtenerPorEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ExisteEmailAsync(string email, int? excluirId = null, CancellationToken ct = default);
    Task<Usuario> CrearAsync(Usuario usuario, CancellationToken ct = default);
    Task<Usuario> ActualizarAsync(Usuario usuario, CancellationToken ct = default);
    Task EliminarAsync(int id, CancellationToken ct = default);
}
