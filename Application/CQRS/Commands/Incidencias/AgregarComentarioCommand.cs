using Application.CQRS.Core;
using Application.DTOS.Incidencias;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Incidencias;

// ─────────────────────────────────────────────────────────────────────────────
// 8. AGREGAR COMENTARIO
// Cualquier usuario involucrado puede comentar.
// ─────────────────────────────────────────────────────────────────────────────

public record AgregarComentarioCommand(
    Guid PublicIdIncidencia,
    string Mensaje,
    bool EsInterno,
    int UsuarioId   // viene del JWT
) : ICommand<ComentarioDto>;

public class AgregarComentarioHandler
    : ICommandHandler<AgregarComentarioCommand, ComentarioDto> {
    private readonly IIncidenciaRepository _repo;
    private readonly IUsuarioRepository _usuarioRepo;

    public AgregarComentarioHandler(
        IIncidenciaRepository repo,
        IUsuarioRepository usuarioRepo) {
        _repo = repo;
        _usuarioRepo = usuarioRepo;
    }

    public async Task<ComentarioDto> HandleAsync(
        AgregarComentarioCommand cmd, CancellationToken ct = default) {
        var incidencia = await _repo.ObtenerPorPublicIdAsync(cmd.PublicIdIncidencia, ct)
            ?? throw new NotFoundException(nameof(Incidencia), cmd.PublicIdIncidencia);

        var usuario = await _usuarioRepo.ObtenerPorIdAsync(cmd.UsuarioId, ct)
            ?? throw new NotFoundException(nameof(Usuario), cmd.UsuarioId);

        var ahora = DateTime.Now;

        var comentario = new ComentarioIncidencia {
            IncidenciaId = incidencia.IncidenciaId,
            UsuarioId = cmd.UsuarioId,
            Mensaje = cmd.Mensaje,
            EsInterno = cmd.EsInterno,
            FechaComentario = ahora
        };

        await _repo.AgregarComentarioAsync(comentario, ct);

        return new ComentarioDto(
            comentario.Mensaje,
            comentario.EsInterno,
            $"{usuario.Nombre} {usuario.Apellidos}",
            usuario.PublicId,
            comentario.FechaComentario
        );
    }
}

