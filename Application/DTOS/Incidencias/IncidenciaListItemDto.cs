using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Incidencias;

public record IncidenciaListItemDto(
    Guid PublicId,
    string NumeroTicket,
    string Titulo,
    string Categoria,
    string Prioridad,
    byte NivelPrioridad,
    string Estado,
    bool EsEstadoFinal,
    string Solicitante,
    string? Tecnico,
    DateTime FechaRegistro,
    DateTime? FechaLimiteResolucion,
    bool? CumpleSla,
    bool ResueltoEnPrimerContacto
);
