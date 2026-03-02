using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface IIncidenciaRepository {
    // ── Consultas ─────────────────────────────────────────────────────────────
    Task<IEnumerable<Incidencia>> ObtenerTodasAsync(CancellationToken ct = default);
    Task<Incidencia?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Incidencia?> ObtenerPorPublicIdAsync(Guid publicId, CancellationToken ct = default);
    Task<IEnumerable<Incidencia>> ObtenerPorSolicitanteAsync(int solicitanteId, CancellationToken ct = default);
    Task<IEnumerable<Incidencia>> ObtenerPorTecnicoAsync(int tecnicoId, CancellationToken ct = default);
    Task<int> ContarPorEstadoAsync(int estadoId, CancellationToken ct = default);

    // ── Persistencia ──────────────────────────────────────────────────────────
    Task<Incidencia> CrearAsync(Incidencia incidencia, CancellationToken ct = default);
    Task<Incidencia> ActualizarAsync(Incidencia incidencia, CancellationToken ct = default);

    // ── Historial ─────────────────────────────────────────────────────────────
    Task AgregarHistorialAsync(HistorialIncidencia historial, CancellationToken ct = default);
    Task<IEnumerable<HistorialIncidencia>> ObtenerHistorialAsync(int incidenciaId, CancellationToken ct = default);

    // ── Comentarios ───────────────────────────────────────────────────────────
    Task AgregarComentarioAsync(ComentarioIncidencia comentario, CancellationToken ct = default);
    Task<IEnumerable<ComentarioIncidencia>> ObtenerComentariosAsync(int incidenciaId, CancellationToken ct = default);
}
