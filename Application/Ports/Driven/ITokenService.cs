using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface ITokenService {
    string GenerarToken(Usuario usuario);
    int? ObtenerUsuarioId(string token);
}
