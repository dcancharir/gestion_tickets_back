using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Exceptions;

/// <summary>Se lanza ante datos inválidos → HTTP 400</summary>
public class ValidationException : Exception {
    public ValidationException(string mensaje)
        : base(mensaje) { }
}
