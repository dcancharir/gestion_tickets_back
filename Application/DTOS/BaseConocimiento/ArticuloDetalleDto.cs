using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.BaseConocimiento;

public record ArticuloDetalleDto(
    Guid PublicId,
    string Titulo,
    string Problema,
    string Solucion,
    string? Categoria,
    int? CategoriaId,
    string CreadoPor,
    DateTime FechaCreacion,
    bool Activo
);