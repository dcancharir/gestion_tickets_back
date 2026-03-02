using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Configuracion;

public record CrearNivelPrioridadDto(string Nombre, byte Nivel, int TiempoRespuestaMin, int TiempoResolucionMin);
