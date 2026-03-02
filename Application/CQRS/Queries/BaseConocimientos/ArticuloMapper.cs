using Application.DTOS.BaseConocimiento;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Queries.BaseConocimientos;

internal static class ArticuloMapper {
    internal static ArticuloListItemDto ToListItem(BaseConocimiento b) => new(
        b.PublicId,
        b.Titulo,
        b.Categoria?.Nombre,
        $"{b.CreadoPor.Nombre} {b.CreadoPor.Apellidos}",
        b.FechaCreacion,
        b.Activo
    );

    internal static ArticuloDetalleDto ToDetalle(BaseConocimiento b) => new(
        b.PublicId,
        b.Titulo,
        b.Problema,
        b.Solucion,
        b.Categoria?.Nombre,
        b.CategoriaId,
        $"{b.CreadoPor.Nombre} {b.CreadoPor.Apellidos}",
        b.FechaCreacion,
        b.Activo
    );
}
