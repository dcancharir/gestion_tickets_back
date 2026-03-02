using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Configuracion;

public record ActualizarNivelPrioridadDto(string Nombre, byte Nivel, int TiempoRespuestaMin, int TiempoResolucionMin);
