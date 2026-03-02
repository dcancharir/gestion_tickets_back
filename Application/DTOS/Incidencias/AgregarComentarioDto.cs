using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Incidencias;

public record AgregarComentarioDto(
    string Mensaje,
    bool EsInterno
);
