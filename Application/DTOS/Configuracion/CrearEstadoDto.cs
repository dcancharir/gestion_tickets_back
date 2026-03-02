using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Configuracion;

public record CrearEstadoDto(string Nombre, string? Descripcion, bool EsEstadoFinal);
