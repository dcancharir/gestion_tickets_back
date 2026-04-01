using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Auth;

public record TokenResponseDto(
    string Token,
    DateTime Expiracion,
    Guid PublicId,
    string Nombre,
    string Apellidos,
    string Email,
    string Rol
);
