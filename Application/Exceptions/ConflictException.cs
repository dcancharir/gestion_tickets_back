using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Exceptions;

/// <summary>Se lanza ante conflictos de negocio → HTTP 409</summary>
public class ConflictException : Exception {
    public ConflictException(string mensaje)
        : base(mensaje) { }
}
