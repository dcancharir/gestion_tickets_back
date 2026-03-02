using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Configuracion;

public record NivelPrioridadDto(
    int PrioridadId,
    string Nombre,
    byte Nivel,
    int TiempoRespuestaMin,
    int TiempoResolucionMin
);
