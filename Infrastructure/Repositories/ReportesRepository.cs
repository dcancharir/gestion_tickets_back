using Application.DTOS.Reportes;
using Application.Ports.Driven;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReportesRepository : IReportesRepository {
    private readonly ApplicationDbContext _db;
    public ReportesRepository(ApplicationDbContext db) => _db = db;

    public async Task<ReportesDto> ObtenerReportesAsync(
        DateTime desde, DateTime hasta, CancellationToken ct = default) {

        // Fin del día para incluir tickets del último día completo
        var hastaFin = hasta.Date.AddDays(1).AddTicks(-1);

        var incidencias = await _db.Incidencias
            .AsNoTracking()
            .Include(i => i.EstadoIncidencia)
            .Where(i => i.FechaRegistro >= desde && i.FechaRegistro <= hastaFin)
            .ToListAsync(ct);

        var resueltas = incidencias.Where(i => i.FechaResolucion.HasValue).ToList();
        var cerradas  = incidencias.Where(i => i.EstadoIncidencia.Nombre == "Cerrado").ToList();

        // ── D1: Tiempo de respuesta ───────────────────────────────────────────
        var conRespuesta = incidencias.Where(i => i.FechaPrimeraRespuesta.HasValue).ToList();
        double? tRespuesta = conRespuesta.Any()
            ? Math.Round(conRespuesta.Average(i =>
                (i.FechaPrimeraRespuesta!.Value - i.FechaRegistro).TotalMinutes), 2)
            : null;

        // ── D1: Tiempo de resolución ──────────────────────────────────────────
        double? tResolucion = resueltas.Any()
            ? Math.Round(resueltas.Average(i =>
                (i.FechaResolucion!.Value - i.FechaRegistro).TotalMinutes), 2)
            : null;

        // ── D1: Cumplimiento SLA ──────────────────────────────────────────────
        var resueltasConSla = resueltas.Where(i => i.FechaLimiteResolucion.HasValue).ToList();
        double? pctSla = resueltasConSla.Any()
            ? Math.Round(100.0 * resueltasConSla.Count(i =>
                i.FechaResolucion <= i.FechaLimiteResolucion) / resueltasConSla.Count, 2)
            : null;

        // ── D2: Valoración promedio ───────────────────────────────────────────
        var incidenciaIds = incidencias.Select(i => i.IncidenciaId).ToList();
        var valoraciones  = await _db.ValoracionesTicket
            .AsNoTracking()
            .Where(v => incidenciaIds.Contains(v.IncidenciaId))
            .Select(v => (double)v.Puntuacion)
            .ToListAsync(ct);
        double? valoracionProm = valoraciones.Any()
            ? Math.Round(valoraciones.Average(), 2)
            : null;

        // ── D2: Tasa de reaperturas ───────────────────────────────────────────
        int reabiertas = incidencias.Count(i => i.EstadoIncidencia.Nombre == "Reabierto");
        int baseReaperturas = resueltas.Count + cerradas.Count + reabiertas;
        double? tasaReaperturas = baseReaperturas > 0
            ? Math.Round(100.0 * reabiertas / baseReaperturas, 2)
            : null;

        // ── D3: Comentarios por ticket ────────────────────────────────────────
        double? promComentarios = null;
        if (incidencias.Any()) {
            var conteoComentarios = await _db.ComentariosIncidencia
                .AsNoTracking()
                .Where(c => incidenciaIds.Contains(c.IncidenciaId))
                .GroupBy(c => c.IncidenciaId)
                .Select(g => g.Count())
                .ToListAsync(ct);
            // Tickets sin comentarios cuentan como 0
            double totalComentarios = conteoComentarios.Sum();
            promComentarios = Math.Round(totalComentarios / incidencias.Count, 2);
        }

        // ── D3: Actualizaciones de historial por ticket ───────────────────────
        double? promHistorial = null;
        if (incidencias.Any()) {
            var conteoHistorial = await _db.HistorialIncidencias
                .AsNoTracking()
                .Where(h => incidenciaIds.Contains(h.IncidenciaId))
                .GroupBy(h => h.IncidenciaId)
                .Select(g => g.Count())
                .ToListAsync(ct);
            double totalHistorial = conteoHistorial.Sum();
            promHistorial = Math.Round(totalHistorial / incidencias.Count, 2);
        }

        return new ReportesDto(
            TotalTickets:                          incidencias.Count,
            TicketsResueltos:                      resueltas.Count,
            TicketsCerrados:                       cerradas.Count,
            TiempoPromedioRespuestaMinutos:        tRespuesta,
            TiempoPromedioResolucionMinutos:       tResolucion,
            PorcentajeCumplimientoSla:             pctSla,
            ValoracionPromedio:                    valoracionProm,
            TasaReaperturasPct:                    tasaReaperturas,
            PromedioComentariosPorTicket:          promComentarios,
            PromedioActualizacionesHistorialPorTicket: promHistorial
        );
    }

    public async Task<ReporteDistribucionDto> ObtenerDistribucionAsync(
        DateTime desde, DateTime hasta, CancellationToken ct = default)
        => await ObtenerDistribucionAsync(ct);

    public async Task<ReporteDistribucionDto> ObtenerDistribucionAsync(
        CancellationToken ct = default) {

        // ── Por técnico ───────────────────────────────────────────────────────
        var porTecnico = await _db.Incidencias
            .AsNoTracking()
            .Include(i => i.TecnicoAsignado)
            .Include(i => i.EstadoIncidencia)
            .Where(i => i.TecnicoAsignadoId != null)
            .GroupBy(i => new { i.TecnicoAsignadoId, i.TecnicoAsignado!.Nombre, i.TecnicoAsignado.Apellidos })
            .Select(g => new {
                TecnicoId    = g.Key.TecnicoAsignadoId!.Value,
                Nombre       = g.Key.Nombre + " " + g.Key.Apellidos,
                Total        = g.Count(),
                Resueltos    = g.Count(i => i.FechaResolucion != null),
                Cerrados     = g.Count(i => i.EstadoIncidencia.Nombre == "Cerrado"),
                TiempoPromMs = g.Where(i => i.FechaResolucion != null)
                                .Average(i => (double?)(EF.Functions.DateDiffSecond(i.FechaRegistro, i.FechaResolucion!.Value)))
            })
            .OrderByDescending(g => g.Total)
            .ToListAsync(ct);

        var tecnicoList = porTecnico.Select(g => new TicketPorTecnicoDto(
            g.TecnicoId,
            g.Nombre,
            g.Total,
            g.Resueltos,
            g.Cerrados,
            g.Total - g.Resueltos - g.Cerrados < 0 ? 0 : g.Total - g.Resueltos - g.Cerrados,
            g.TiempoPromMs.HasValue ? Math.Round(g.TiempoPromMs.Value / 60.0, 2) : null
        )).ToList();

        // ── Por sede ──────────────────────────────────────────────────────────
        var porSede = await _db.Incidencias
            .AsNoTracking()
            .Include(i => i.Sede)
            .Include(i => i.EstadoIncidencia)
            .GroupBy(i => new { i.SedeId, i.Sede!.Nombre, i.Sede.TipoSede })
            .Select(g => new {
                SedeId   = g.Key.SedeId,
                Nombre   = g.Key.Nombre,
                TipoSede = g.Key.TipoSede,
                Total    = g.Count(),
                Resueltos = g.Count(i => i.FechaResolucion != null),
                Cerrados  = g.Count(i => i.EstadoIncidencia.Nombre == "Cerrado"),
            })
            .OrderByDescending(g => g.Total)
            .ToListAsync(ct);

        var sedeList = porSede.Select(g => new TicketPorSedeDto(
            g.SedeId,
            g.Nombre,
            g.TipoSede,
            g.Total,
            g.Resueltos,
            g.Cerrados,
            g.Total - g.Resueltos - g.Cerrados < 0 ? 0 : g.Total - g.Resueltos - g.Cerrados
        )).ToList();

        return new ReporteDistribucionDto(tecnicoList, sedeList);
    }
}
