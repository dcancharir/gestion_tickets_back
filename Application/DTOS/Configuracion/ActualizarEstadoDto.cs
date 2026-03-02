using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Configuracion;

public record ActualizarEstadoDto(string Nombre, string? Descripcion, bool EsEstadoFinal);

