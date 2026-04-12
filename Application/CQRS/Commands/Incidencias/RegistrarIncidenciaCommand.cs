using Application.CQRS.Core;
using Application.CQRS.Queries.Incidencias;
using Application.DTOS.Incidencias;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Incidencias;

// ─────────────────────────────────────────────────────────────────────────────
// 1. REGISTRAR INCIDENCIA
// Cualquier rol puede crear. SolicitanteId viene del JWT.
// ─────────────────────────────────────────────────────────────────────────────

public record RegistrarIncidenciaCommand(
    string Titulo,
    string Descripcion,
    int CategoriaId,
    string CanalReporte,
    byte Impacto,
    byte Urgencia,
    int PrioridadId,
    int SolicitanteId,// resuelto desde el JWT en el controller
    int SedeId

) : ICommand<IncidenciaListItemDto>;

public class RegistrarIncidenciaHandler
    : ICommandHandler<RegistrarIncidenciaCommand, IncidenciaListItemDto> {
    private readonly IIncidenciaRepository _repo;

    public RegistrarIncidenciaHandler(IIncidenciaRepository repo) => _repo = repo;

    public async Task<IncidenciaListItemDto> HandleAsync(
        RegistrarIncidenciaCommand cmd, CancellationToken ct = default) {
        // Generar número de ticket: TKT-YYYY-NNNNN
        var conteo = await _repo.ContarPorEstadoAsync(0, ct); // total general
        var numeroTicket = $"TKT-{DateTime.Now.Year}-{(conteo + 1):D5}";
        var ahora = DateTime.Now;

        // Calcular fechas límite SLA según prioridad
        // (los minutos vienen de NivelPrioridad, ya definidos en el seed)
        var incidencia = new Incidencia {
            NumeroTicket = numeroTicket,
            Titulo = cmd.Titulo,
            Descripcion = cmd.Descripcion,
            CategoriaId = cmd.CategoriaId,
            CanalReporte = cmd.CanalReporte,
            Impacto = cmd.Impacto,
            Urgencia = cmd.Urgencia,
            PrioridadId = cmd.PrioridadId,
            SolicitanteId = cmd.SolicitanteId,
            EstadoId = 1, // Registrado
            FechaRegistro = ahora,
            FechaUltimaActualizacion = ahora,
            SedeId = cmd.SedeId,
        };

        var creada = await _repo.CrearAsync(incidencia, ct);

        // Registrar en historial
        await _repo.AgregarHistorialAsync(new HistorialIncidencia {
            IncidenciaId = creada.IncidenciaId,
            UsuarioId = cmd.SolicitanteId,
            Accion = "Registro",
            EstadoNuevo = "Registrado",
            Detalle = $"Ticket {numeroTicket} registrado vía {cmd.CanalReporte}"
        }, ct);

        var completa = await _repo.ObtenerPorIdAsync(creada.IncidenciaId, ct);
        return IncidenciaMapper.ToListItem(completa!);
    }
}
