using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Configuracion;

public record EstadoIncidenciaDto(int EstadoId, string Nombre, string? Descripcion, bool EsEstadoFinal);
