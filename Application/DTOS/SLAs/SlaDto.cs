using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.SLAs;

public record SlaDto(
    int SlaId,
    int CategoriaId,
    string CategoriaNombre,
    int PrioridadId,
    string PrioridadNombre,
    int TiempoRespuestaMin,
    int TiempoResolucionMin,
    bool Activo
);
