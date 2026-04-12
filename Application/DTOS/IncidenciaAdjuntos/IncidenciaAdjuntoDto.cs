using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.IncidenciaAdjuntos;

public record IncidenciaAdjuntoDto (
        int IdIncidenciaAdjunto,
        int IdIncidencia,
        string Nombre,
        string RutaContenedora,
        string NombreReal,
        DateTime FechaCreacion
    );
