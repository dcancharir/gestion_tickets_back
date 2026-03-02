using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Categorias;

public record CategoriaDto(int CategoriaId, string Nombre, string? Descripcion, bool Activo);