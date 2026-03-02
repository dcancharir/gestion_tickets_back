using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Incidencias;

public record CambiarEstadoDto(
    int NuevoEstadoId,
    string Detalle
);
