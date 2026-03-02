using Application.CQRS.Core;
using Application.CQRS.Queries.BaseConocimientos;
using Application.DTOS.BaseConocimiento;
using Application.Exceptions;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.BaseConocimientos;

public record ActualizarArticuloCommand(
    Guid PublicId,
    string Titulo,
    string Problema,
    string Solucion,
    int? CategoriaId,
    bool Activo
) : ICommand<ArticuloDetalleDto>;

public class ActualizarArticuloHandler : ICommandHandler<ActualizarArticuloCommand, ArticuloDetalleDto> {
    private readonly IBaseConocimientoRepository _repo;
    public ActualizarArticuloHandler(IBaseConocimientoRepository repo) => _repo = repo;

    public async Task<ArticuloDetalleDto> HandleAsync(
        ActualizarArticuloCommand cmd, CancellationToken ct = default) {
        var articulo = await _repo.ObtenerPorPublicIdAsync(cmd.PublicId, ct)
            ?? throw new NotFoundException(nameof(BaseConocimiento), cmd.PublicId);

        articulo.Titulo = cmd.Titulo;
        articulo.Problema = cmd.Problema;
        articulo.Solucion = cmd.Solucion;
        articulo.CategoriaId = cmd.CategoriaId;
        articulo.Activo = cmd.Activo;

        await _repo.ActualizarAsync(articulo, ct);
        var actualizado = await _repo.ObtenerPorIdAsync(articulo.ArticuloId, ct);
        return ArticuloMapper.ToDetalle(actualizado!);
    }
}
