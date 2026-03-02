using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.SLAs;

public record CrearSlaDto(int CategoriaId, int PrioridadId, int TiempoRespuestaMin, int TiempoResolucionMin);
