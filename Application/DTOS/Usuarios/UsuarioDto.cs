using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Usuarios;

public record UsuarioDto(
    Guid PublicId,       // ← Guid expuesto al frontend
    string Nombre,
    string Apellidos,
    string Email,
    int RolId,
    string RolNombre,
    bool Activo,
    DateTime FechaCreacion
// UsuarioId (int) nunca aparece aquí
);
