using Application.CQRS.Core;
using Application.DTOS.Permiso;
using Application.Ports.Driven;
using Domain.Entities;

namespace Application.CQRS.Commands.Permisos;

public record SincronizarPermisosCommand(
    List<CrearPermisoDto> Permisos
) : ICommand<bool>;

public class SincronizarPermisosHandler : ICommandHandler<SincronizarPermisosCommand, bool>
{
    private readonly IPermisoRepository _repo;

    public SincronizarPermisosHandler(IPermisoRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> HandleAsync(
        SincronizarPermisosCommand command, CancellationToken ct = default)
    {
        var permisosBd = (await _repo.ObtenerTodosAsync(ct)).ToList();

        // Crear clave única para comparar
        string Key(string nombre, string controlador, string tipo) =>
            $"{nombre}|{controlador}|{tipo}".ToLowerInvariant();

        var permisosBdMap = permisosBd.ToDictionary(
            x => Key(x.Nombre, x.Controlador, x.Tipo),
            x => x
        );

        var permisosInputKeys = command.Permisos
            .Select(p => Key(p.nombre, p.controlador, p.tipo))
            .ToHashSet();

        // INSERTAR: los que no existen en BD
        var listaInsertar = command.Permisos
            .Where(p => !permisosBdMap.ContainsKey(
                Key(p.nombre, p.controlador, p.tipo)))
            .Select(p => new Permiso
            {
                Nombre = p.nombre,
                Controlador = p.controlador,
                Tipo = p.tipo
            })
            .ToList();

        // ELIMINAR: los que están en BD pero no en input
        var listaEliminar = permisosBd
            .Where(p => !permisosInputKeys.Contains(
                Key(p.Nombre, p.Controlador, p.Tipo)))
            .Select(p => p.PermisoId)
            .ToList();

        if (listaInsertar.Count != 0)
            await _repo.CrearRangoAsync(listaInsertar, ct);

        if (listaEliminar.Count != 0)
            await _repo.EliminarRangoAsync(listaEliminar, ct);

        return true;
    }

}