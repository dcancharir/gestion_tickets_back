using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.BaseConocimiento;

public record CrearArticuloDto(
    string Titulo,
    string Problema,
    string Solucion,
    int? CategoriaId
// CreadoPorId viene del JWT
);
