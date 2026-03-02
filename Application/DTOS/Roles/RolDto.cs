using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Roles;

public record RolDto(int RolId, string Nombre, string? Descripcion);
