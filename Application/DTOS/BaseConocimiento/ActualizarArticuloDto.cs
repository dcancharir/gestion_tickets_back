using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.BaseConocimiento;

public record ActualizarArticuloDto(
    string Titulo,
    string Problema,
    string Solucion,
    int? CategoriaId,
    bool Activo
);
