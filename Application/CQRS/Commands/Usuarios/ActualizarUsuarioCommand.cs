using Application.CQRS.Core;
using Application.CQRS.Queries.Usuarios;
using Application.DTOS.Usuarios;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Usuarios;

// ── Command ───────────────────────────────────────────────────────────────────

public record ActualizarUsuarioCommand(
    Guid PublicId,   // ← viene del frontend
    string Nombre,
    string Apellidos,
    string Email,
    int RolId,
    bool Activo
) : ICommand<UsuarioDto>;

// ── Handler ───────────────────────────────────────────────────────────────────

public class ActualizarUsuarioHandler : ICommandHandler<ActualizarUsuarioCommand, UsuarioDto> {
    private readonly IUsuarioRepository _repo;

    public ActualizarUsuarioHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<UsuarioDto> HandleAsync(
        ActualizarUsuarioCommand command,
        CancellationToken ct = default) {
        // 1. Resolver Guid → entidad (obtiene el int internamente)
        var usuario = await _repo.ObtenerPorPublicIdAsync(command.PublicId, ct)
            ?? throw new NotFoundException(nameof(Usuario), command.PublicId);

        //// 2. Validar email
        //var emailOcupado = await _repo.ExisteEmailAsync(
        //    command.Email, excluirId: usuario.UsuarioId, ct: ct);

        //if(emailOcupado)
        //    throw new ConflictException($"El email '{command.Email}' ya está en uso.");

        // 3. Aplicar cambios
        usuario.Nombre = command.Nombre;
        usuario.Apellidos = command.Apellidos;
        usuario.Email = command.Email;
        usuario.RolId = command.RolId;
        usuario.Activo = command.Activo;

        var actualizado = await _repo.ActualizarAsync(usuario, ct);
        var conRol = await _repo.ObtenerPorIdAsync(actualizado.UsuarioId, ct);

        return ObtenerUsuarioPorPublicIdHandler.ToDto(conRol!);
    }
}
