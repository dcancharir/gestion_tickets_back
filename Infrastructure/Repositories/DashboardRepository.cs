using Application.DTOS.Dashboard;
using Application.Ports.Driven;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository {
    private readonly ApplicationDbContext _db;

    public DashboardRepository(ApplicationDbContext db) => _db = db;

    public async Task<DashboardKpiDto> ObtenerKpisAsync(CancellationToken ct = default) {
        // Traer todas las incidencias con sus relaciones necesarias
        // AsNoTracking para máximo rendimiento en lectura
        var incidencias = await _db.Incidencias
            .AsNoTracking()
            .Include(i => i.EstadoIncidencia)
            .Include(i => i.Categoria)
            .Include(i => i.NivelPrioridad)
            .Include(i => i.TecnicoAsignado)
            .ToListAsync(ct);

        // ── Contadores por estado ─────────────────────────────────────────────

        var estados = new ResumenEstadosDto(
            Total: incidencias.Count,
            Registrados: incidencias.Count(i => i.EstadoIncidencia.Nombre == "Registrado"),
            Asignados: incidencias.Count(i => i.EstadoIncidencia.Nombre == "Asignado"),
            EnDiagnostico: incidencias.Count(i => i.EstadoIncidencia.Nombre == "En Diagnóstico"),
            EnProgreso: incidencias.Count(i => i.EstadoIncidencia.Nombre == "En Progreso"),
            Pendientes: incidencias.Count(i => i.EstadoIncidencia.Nombre == "Pendiente"),
            Resueltos: incidencias.Count(i => i.EstadoIncidencia.Nombre == "Resuelto"),
            Cerrados: incidencias.Count(i => i.EstadoIncidencia.Nombre == "Cerrado"),
            Reabiertas: incidencias.Count(i => i.EstadoIncidencia.Nombre == "Reabierto"),
            Cancelados: incidencias.Count(i => i.EstadoIncidencia.Nombre == "Cancelado")
        );

        // ── Conjuntos para calcular KPIs ──────────────────────────────────────

        // Tickets que ya tienen fecha de resolución (base para MTTR y SLA)
        var resueltas = incidencias
            .Where(i => i.FechaResolucion.HasValue)
            .ToList();

        // Tickets cerrados (base para reincidencia)
        var cerradas = incidencias
            .Where(i => i.EstadoIncidencia.Nombre == "Cerrado")
            .ToList();

        // ── KPI 1: MTTR — Mean Time To Resolve ───────────────────────────────
        // Promedio de minutos desde FechaRegistro hasta FechaResolucion

        double? mttr = resueltas.Any()
            ? Math.Round(
                resueltas.Average(i =>
                    (i.FechaResolucion!.Value - i.FechaRegistro).TotalMinutes),
                2)
            : null;

        // ── KPI 2: MTTR Respuesta — Mean Time To First Response ───────────────
        // Promedio de minutos desde FechaRegistro hasta FechaPrimeraRespuesta

        var conRespuesta = incidencias
            .Where(i => i.FechaPrimeraRespuesta.HasValue)
            .ToList();

        double? mttrRespuesta = conRespuesta.Any()
            ? Math.Round(
                conRespuesta.Average(i =>
                    (i.FechaPrimeraRespuesta!.Value - i.FechaRegistro).TotalMinutes),
                2)
            : null;

        // ── KPI 3: Cumplimiento SLA ───────────────────────────────────────────
        // % de tickets resueltos antes de FechaLimiteResolucion

        var resueltasConSla = resueltas
            .Where(i => i.FechaLimiteResolucion.HasValue)
            .ToList();

        double? pctSla = resueltasConSla.Any()
            ? Math.Round(
                100.0 * resueltasConSla.Count(i =>
                    i.FechaResolucion <= i.FechaLimiteResolucion)
                / resueltasConSla.Count,
                2)
            : null;

        // ── KPI 4: Resolución en Primer Contacto ──────────────────────────────
        // % de tickets resueltos sin reasignaciones ni escalamientos

        double? pctPrimerContacto = resueltas.Any()
            ? Math.Round(
                100.0 * resueltas.Count(i => i.ResueltoEnPrimerContacto)
                / resueltas.Count,
                2)
            : null;

        // ── KPI 5: Tasa de Reincidencia ───────────────────────────────────────
        // % de tickets reabiertos sobre el total de cerrados

        var reabiertas = incidencias.Count(i => i.EstadoIncidencia.Nombre == "Reabierto");

        double? pctReincidencia = cerradas.Any()
            ? Math.Round(100.0 * reabiertas / cerradas.Count, 2)
            : null;

        var kpisItil = new KpisItilDto(
            MttrPromedioMinutos: mttr,
            MttrRespuestaPromedioMinutos: mttrRespuesta,
            PorcentajeCumplimientoSla: pctSla,
            PorcentajeResolucionPrimerContacto: pctPrimerContacto,
            PorcentajeReincidencia: pctReincidencia
        );

        // ── Distribución por categoría ────────────────────────────────────────

        var porCategoria = incidencias
            .GroupBy(i => i.Categoria.Nombre)
            .Select(g => new ConteoDto(g.Key, g.Count()))
            .OrderByDescending(x => x.Total)
            .ToList();

        // ── Distribución por prioridad ────────────────────────────────────────

        var porPrioridad = incidencias
            .GroupBy(i => new { i.NivelPrioridad.Nombre, i.NivelPrioridad.Nivel })
            .Select(g => new ConteoDto(g.Key.Nombre, g.Count()))
            .OrderBy(x => x.Nombre)
            .ToList();

        // ── KPIs por técnico ──────────────────────────────────────────────────

        var topTecnicos = incidencias
            .Where(i => i.TecnicoAsignado is not null)
            .GroupBy(i => new {
                Nombre = $"{i.TecnicoAsignado!.Nombre} {i.TecnicoAsignado.Apellidos}"
            })
            .Select(g => {
                var asignados = g.ToList();
                var resueltasTec = asignados.Where(i => i.FechaResolucion.HasValue).ToList();
                var cerradasTec = asignados.Where(i => i.EstadoIncidencia.Nombre == "Cerrado").ToList();
                var conSlaTec = resueltasTec.Where(i => i.FechaLimiteResolucion.HasValue).ToList();

                double? mttrTec = resueltasTec.Any()
                    ? Math.Round(
                        resueltasTec.Average(i =>
                            (i.FechaResolucion!.Value - i.FechaRegistro).TotalMinutes),
                        2)
                    : null;

                double? slaTec = conSlaTec.Any()
                    ? Math.Round(
                        100.0 * conSlaTec.Count(i =>
                            i.FechaResolucion <= i.FechaLimiteResolucion)
                        / conSlaTec.Count,
                        2)
                    : null;

                double? primerContactoTec = resueltasTec.Any()
                    ? Math.Round(
                        100.0 * resueltasTec.Count(i => i.ResueltoEnPrimerContacto)
                        / resueltasTec.Count,
                        2)
                    : null;

                return new KpiTecnicoDto(
                    Tecnico: g.Key.Nombre,
                    TotalAsignados: asignados.Count,
                    Resueltos: resueltasTec.Count,
                    Cerrados: cerradasTec.Count,
                    MttrPromedioMinutos: mttrTec,
                    PorcentajeCumplimientoSla: slaTec,
                    PorcentajePrimerContacto: primerContactoTec
                );
            })
            .OrderByDescending(t => t.Resueltos)
            .ToList();

        return new DashboardKpiDto(
            Estados: estados,
            KpisItil: kpisItil,
            PorCategoria: porCategoria,
            PorPrioridad: porPrioridad,
            TopTecnicos: topTecnicos
        );
    }
}
