using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Exceptions;

/// <summary>Se lanza cuando no se encuentra una entidad → HTTP 404</summary>
public class NotFoundException : Exception {
    public NotFoundException(string entidad, object id)
        : base($"{entidad} con Id '{id}' no fue encontrado.") { }
}
