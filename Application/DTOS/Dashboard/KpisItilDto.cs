using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Dashboard;

public record KpisItilDto(
    // Mean Time To Resolve — promedio de minutos desde registro hasta resolución
    double? MttrPromedioMinutos,

    // Mean Time To Respond — promedio de minutos hasta primera respuesta
    double? MttrRespuestaPromedioMinutos,

    // % tickets resueltos dentro del tiempo límite SLA
    double? PorcentajeCumplimientoSla,

    // % tickets resueltos en el primer contacto
    double? PorcentajeResolucionPrimerContacto,

    // % tickets reabiertos sobre el total de cerrados
    double? PorcentajeReincidencia
);
