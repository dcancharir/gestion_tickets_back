using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Incidencias;

// CerrarIncidenciaDto no necesita body — toda la info viene de la ruta y el JWT.
// Se deja como referencia vacía por si se quiere añadir un comentario de cierre.
public record CerrarIncidenciaDto(
    string? Comentario   // opcional: mensaje de confirmación del cierre
);
