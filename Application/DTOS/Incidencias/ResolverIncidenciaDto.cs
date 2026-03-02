using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Incidencias;

public record ResolverIncidenciaDto(
    string SolucionAplicada,
    bool ResueltoEnPrimerContacto
);
