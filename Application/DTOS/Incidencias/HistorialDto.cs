using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Incidencias;

public record HistorialDto(
    string Accion,
    string? EstadoAnterior,
    string? EstadoNuevo,
    string? Detalle,
    string Usuario,
    DateTime FechaAccion
);