using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.SLAs;

public record ActualizarSlaDto(int CategoriaId, int PrioridadId, int TiempoRespuestaMin, int TiempoResolucionMin, bool Activo);

