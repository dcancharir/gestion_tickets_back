using Application.CQRS.Core;
using Application.CQRS.Queries.BaseConocimientos;
using Application.DTOS.BaseConocimiento;
using Application.Ports.Driven;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Commands.BaseConocimientos;

public record CrearArticuloCommand(
    string Titulo,
    string Problema,
    string Solucion,
    int? CategoriaId,
    int CreadoPorId   // viene del JWT
) : ICommand<ArticuloDetalleDto>;

public class CrearArticuloHandler : ICommandHandler<CrearArticuloCommand, ArticuloDetalleDto> {
    private readonly IBaseConocimientoRepository _repo;
    public CrearArticuloHandler(IBaseConocimientoRepository repo) => _repo = repo;

    public async Task<ArticuloDetalleDto> HandleAsync(
        CrearArticuloCommand cmd, CancellationToken ct = default) {
        var articulo = new BaseConocimiento {
            Titulo = cmd.Titulo,
            Problema = cmd.Problema,
            Solucion = cmd.Solucion,
            CategoriaId = cmd.CategoriaId,
            CreadoPorId = cmd.CreadoPorId,
            FechaCreacion = DateTime.Now,
            Activo = true
        };

        var creado = await _repo.CrearAsync(articulo, ct);
        var conRelaciones = await _repo.ObtenerPorIdAsync(creado.ArticuloId, ct);
        return ArticuloMapper.ToDetalle(conRelaciones!);
    }
}