using Application.CQRS.Core;
using Application.DTOS.Valoracion;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;

namespace Application.CQRS.Commands.Valoracion;

// ── DTOs ──────────────────────────────────────────────────────────────────────

// ── Command ───────────────────────────────────────────────────────────────────

public record ValorarTicketCommand(
    Guid    TicketPublicId,
    int     SolicitanteId,
    byte    Puntuacion,
    string? Comentario
) : ICommand<ValoracionDto>;

// ── Handler ───────────────────────────────────────────────────────────────────

public class ValorarTicketHandler : ICommandHandler<ValorarTicketCommand, ValoracionDto> {
    private readonly IValoracionRepository  _repo;
    private readonly IIncidenciaRepository  _incRepo;

    public ValorarTicketHandler(
        IValoracionRepository repo,
        IIncidenciaRepository incRepo) {
        _repo    = repo;
        _incRepo = incRepo;
    }

    public async Task<ValoracionDto> HandleAsync(
        ValorarTicketCommand command,
        CancellationToken ct = default) {

        // 1. Buscar la incidencia
        var incidencia = await _incRepo.ObtenerPorPublicIdAsync(command.TicketPublicId, ct)
            ?? throw new NotFoundException("Incidencia", command.TicketPublicId);

        // 2. Solo se puede valorar si está Resuelto o Cerrado (EstadoId 6=Resuelto, 7=Cerrado)
        if (incidencia.EstadoId != 6 && incidencia.EstadoId != 7)
            throw new ConflictException("Solo se puede valorar un ticket resuelto o cerrado.");

        // 3. Solo el solicitante puede valorar
        if (incidencia.SolicitanteId != command.SolicitanteId)
            throw new ConflictException("Solo el solicitante puede valorar este ticket.");

        // 4. Verificar que no exista ya una valoración
        var yaExiste = await _repo.ExistePorIncidenciaIdAsync(incidencia.IncidenciaId, ct);
        if (yaExiste)
            throw new ConflictException("Este ticket ya fue valorado.");

        // 5. Validar puntuación (1–5)
        if (command.Puntuacion < 1 || command.Puntuacion > 5)
            throw new ConflictException("La puntuación debe estar entre 1 y 5.");

        // 6. Crear valoración
        var valoracion = new ValoracionTicket {
            IncidenciaId   = incidencia.IncidenciaId,
            SolicitanteId  = command.SolicitanteId,
            Puntuacion     = command.Puntuacion,
            Comentario     = command.Comentario?.Trim(),
            FechaValoracion = DateTime.UtcNow
        };

        var creada = await _repo.CrearAsync(valoracion, ct);

        return new ValoracionDto(
            creada.ValoracionId,
            creada.Puntuacion,
            creada.Comentario,
            creada.FechaValoracion
        );
    }
}
