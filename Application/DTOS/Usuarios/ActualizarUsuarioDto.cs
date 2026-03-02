using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Usuarios;

public record ActualizarUsuarioDto(
    string Nombre,
    string Apellidos,
    string Email,
    int RolId,
    bool Activo
);
