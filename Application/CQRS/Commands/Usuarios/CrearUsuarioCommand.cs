using Application.CQRS.Core;
using Application.DTOS.Usuarios;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.Usuarios;

// ── Command ───────────────────────────────────────────────────────────────────

public record CrearUsuarioCommand(
    string Nombre,
    string Apellidos,
    string Email,
    string Password,
    int RolId
) : ICommand<UsuarioDto>;

// ── Handler ───────────────────────────────────────────────────────────────────

public class CrearUsuarioHandler : ICommandHandler<CrearUsuarioCommand, UsuarioDto> {
    private readonly IUsuarioRepository _repo;

    public CrearUsuarioHandler(IUsuarioRepository repo) {
        _repo = repo;
    }

    public async Task<UsuarioDto> HandleAsync(
        CrearUsuarioCommand command,
        CancellationToken ct = default) {
        // 1. Validar que el email no esté en uso
        var emailOcupado = await _repo.ExisteEmailAsync(command.Email, ct: ct);
        if(emailOcupado)
            throw new ConflictException($"El email '{command.Email}' ya está registrado.");

        // 2. Hashear la contraseña con BCrypt
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);

        // 3. Crear la entidad
        var usuario = new Usuario {
            Nombre = command.Nombre,
            Apellidos = command.Apellidos,
            Email = command.Email,
            PasswordHash = passwordHash,
            RolId = command.RolId,
            Activo = true,
            FechaCreacion = DateTime.Now
        };

        var creado = await _repo.CrearAsync(usuario, ct);

        // 4. Recargar con el Rol incluido para poblar el DTO
        var conRol = await _repo.ObtenerPorIdAsync(creado.UsuarioId, ct)!;

        return new UsuarioDto(
            conRol!.PublicId,
            conRol.Nombre,
            conRol.Apellidos,
            conRol.Email,
            conRol.RolId,
            conRol.Rol.Nombre,
            conRol.Activo,
            conRol.FechaCreacion
        );
    }
}
