using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.BaseConocimiento;

public record ArticuloListItemDto(
    Guid PublicId,
    string Titulo,
    string? Categoria,
    string CreadoPor,
    DateTime FechaCreacion,
    bool Activo
);
