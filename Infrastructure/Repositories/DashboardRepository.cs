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

        // ── KPI: Resueltos últimos 7 días vs 7 días anteriores ────────────────
        var ahora    = DateTime.UtcNow;
        var hace7d   = ahora.AddDays(-7);
        var hace14d  = ahora.AddDays(-14);

        int resueltos7d        = resueltas.Count(i => i.FechaResolucion >= hace7d);
        int resueltosAnterior7d = resueltas.Count(i =>
            i.FechaResolucion >= hace14d && i.FechaResolucion < hace7d);

        var kpisItil = new KpisItilDto(
            MttrPromedioMinutos: mttr,
            MttrRespuestaPromedioMinutos: mttrRespuesta,
            PorcentajeCumplimientoSla: pctSla,
            PorcentajeResolucionPrimerContacto: pctPrimerContacto,
            PorcentajeReincidencia: pctReincidencia,
            Resueltos7d: resueltos7d,
            ResueltosAnterior7d: resueltosAnterior7d
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

    // ─────────────────────────────────────────────────────────────────────────
    // KPIs PERSONALES DEL TÉCNICO
    // Solo incluye tickets asignados a ese técnico.
    // ─────────────────────────────────────────────────────────────────────────
    public async Task<DashboardKpiTecnicoDto> ObtenerKpisTecnicoAsync(
        int tecnicoId, CancellationToken ct = default) {

        var mis = await _db.Incidencias
            .AsNoTracking()
            .Include(i => i.EstadoIncidencia)
            .Include(i => i.NivelPrioridad)
            .Where(i => i.TecnicoAsignadoId == tecnicoId)
            .ToListAsync(ct);

        var hoy = DateTime.Today;

        // ── Contadores ────────────────────────────────────────────────────────
        var resumen = new ResumenTecnicoDto(
            TotalAsignados: mis.Count,
            EnProgreso:     mis.Count(i => i.EstadoId is 2 or 3 or 4),   // Asignado/Diagnóstico/En Progreso
            Pendientes:     mis.Count(i => i.EstadoId == 5),
            ResueltosHoy:   mis.Count(i => i.FechaResolucion.HasValue &&
                                           i.FechaResolucion.Value.Date == hoy),
            Criticos:       mis.Count(i => i.NivelPrioridad.Nivel == 1 &&  // Nivel 1 = Crítico
                                           i.EstadoId != 6 && i.EstadoId != 7)
        );

        // ── KPIs personales ───────────────────────────────────────────────────
        var resueltos = mis.Where(i => i.FechaResolucion.HasValue).ToList();

        double? miMttr = resueltos.Any()
            ? Math.Round(resueltos.Average(i =>
                (i.FechaResolucion!.Value - i.FechaRegistro).TotalMinutes), 2)
            : null;

        var conSla = resueltos.Where(i => i.FechaLimiteResolucion.HasValue).ToList();
        double? miSla = conSla.Any()
            ? Math.Round(100.0 * conSla.Count(i =>
                i.FechaResolucion <= i.FechaLimiteResolucion) / conSla.Count, 2)
            : null;

        double? miPrimerContacto = resueltos.Any()
            ? Math.Round(100.0 * resueltos.Count(i => i.ResueltoEnPrimerContacto)
                / resueltos.Count, 2)
            : null;

        // ── Próximos a vencer SLA (dentro de las próximas 4 horas, no finales) ─
        var limite = DateTime.Now.AddHours(4);
        var proximosVencer = mis
            .Where(i => i.FechaLimiteResolucion.HasValue &&
                        i.FechaLimiteResolucion.Value <= limite &&
                        i.FechaLimiteResolucion.Value >= DateTime.Now &&
                        i.EstadoId != 6 && i.EstadoId != 7)
            .OrderBy(i => i.FechaLimiteResolucion)
            .Select(i => new TicketResumenDto(
                PublicId:            i.PublicId.ToString(),
                NumeroTicket:        i.NumeroTicket,
                Titulo:              i.Titulo,
                Estado:              i.EstadoIncidencia.Nombre,
                Prioridad:           i.NivelPrioridad.Nombre,
                FechaRegistro:       i.FechaRegistro,
                FechaLimiteResolucion: i.FechaLimiteResolucion))
            .ToList();

        // ── Críticos abiertos asignados a este técnico ────────────────────────
        var criticosAbiertos = mis
            .Where(i => i.NivelPrioridad.Nivel == 1 &&
                        i.EstadoId != 6 && i.EstadoId != 7)
            .OrderBy(i => i.FechaRegistro)
            .Select(i => new TicketResumenDto(
                PublicId:            i.PublicId.ToString(),
                NumeroTicket:        i.NumeroTicket,
                Titulo:              i.Titulo,
                Estado:              i.EstadoIncidencia.Nombre,
                Prioridad:           i.NivelPrioridad.Nombre,
                FechaRegistro:       i.FechaRegistro,
                FechaLimiteResolucion: i.FechaLimiteResolucion))
            .ToList();

        return new DashboardKpiTecnicoDto(
            MisTickets:          resumen,
            MiMttrMinutos:       miMttr,
            MiSla:               miSla,
            MiPrimerContacto:    miPrimerContacto,
            ProximosAVencerSla:  proximosVencer,
            CriticosAbiertos:    criticosAbiertos
        );
    }

    // ─────────────────────────────────────────────────────────────────────────
    // TENDENCIA 7 DÍAS
    // Devuelve los últimos 7 días con conteo de registrados y resueltos por día.
    // ─────────────────────────────────────────────────────────────────────────
    public async Task<IEnumerable<TendenciaDiaDto>> ObtenerTendencia7dAsync(
        CancellationToken ct = default) {

        var hoy    = DateTime.UtcNow.Date;
        var inicio = hoy.AddDays(-6); // 7 días incluyendo hoy

        var incidencias = await _db.Incidencias
            .AsNoTracking()
            .Where(i => i.FechaRegistro.Date >= inicio ||
                        (i.FechaResolucion.HasValue && i.FechaResolucion.Value.Date >= inicio))
            .Select(i => new {
                FechaRegistro   = i.FechaRegistro.Date,
                FechaResolucion = i.FechaResolucion.HasValue ? (DateTime?)i.FechaResolucion.Value.Date : null
            })
            .ToListAsync(ct);

        // Generar todos los días del rango aunque no haya datos
        var dias = Enumerable.Range(0, 7)
            .Select(offset => inicio.AddDays(offset))
            .Select(dia => new TendenciaDiaDto(
                Fecha:       dia.ToString("dd/MM"),
                Registrados: incidencias.Count(i => i.FechaRegistro == dia),
                Resueltos:   incidencias.Count(i => i.FechaResolucion.HasValue && i.FechaResolucion!.Value == dia)
            ))
            .ToList();

        return dias;
    }
}
