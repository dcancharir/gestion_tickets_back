using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Categorias;

public record ActualizarCategoriaDto(string Nombre, string? Descripcion, bool Activo);

