using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Ports.Driven;

public interface ISedeRepository {
    Task<IEnumerable<Sede>> ObtenerTodasAsync(CancellationToken ct);
    Task<Sede> CrearAsync(Sede sede, CancellationToken ct);
}