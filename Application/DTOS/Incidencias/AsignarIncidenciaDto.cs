using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Incidencias;
// ── Reemplaza AsignarIncidenciaDto en IncidenciaDto.cs ────────────────────────
// Se elimina el campo AdminId del DTO porque el identificador del ejecutor
// siempre viene del JWT, nunca del body del request.
public record AsignarIncidenciaDto(
    Guid TecnicoPublicId
);
