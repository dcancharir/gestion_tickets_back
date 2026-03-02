using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Incidencias;

public record ComentarioDto(
    string Mensaje,
    bool EsInterno,
    string Usuario,
    Guid UsuarioPublicId,
    DateTime FechaComentario
);
