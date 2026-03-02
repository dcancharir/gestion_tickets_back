using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Usuarios;

public record CrearUsuarioDto(
    string Nombre,
    string Apellidos,
    string Email,
    string Password,
    int RolId
);
