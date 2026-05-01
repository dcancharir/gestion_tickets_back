using Application.CQRS.Commands.Permisos;
using Application.CQRS.Commands.Usuarios;
using Application.CQRS.Core;
using Application.CQRS.Queries.Permisos;
using Application.CQRS.Queries.PermisosRol;
using Application.CQRS.Queries.Usuarios;
using Application.DTOS.Permiso;
using Application.DTOS.PermisoRol;
using Application.DTOS.Permisos;
using Application.DTOS.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PermisoController:ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public PermisoController(IDispatcher dispatcher) => _dispatcher = dispatcher;
    
    [HttpPost("sincronizarpermisos")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> SincronizarPermisos(CancellationToken ct) {
        var permissions = ListControllersAndActions();
        var vistas = ListMenuPermissions();
        if (vistas.Count > 0)
        {
            permissions.AddRange(vistas);
        }

        var command = new SincronizarPermisosCommand(permissions);
        var result = await _dispatcher.SendAsync(command, ct);
        return Ok(result);
    }

    private List<CrearPermisoDto> ListControllersAndActions()
    {
        var permisoDto = new List<CrearPermisoDto>();
        var assembly = Assembly.GetExecutingAssembly();
        var controllerTypes = assembly.GetTypes()
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && type.Name.EndsWith("Controller"))
            .ToList();

        foreach (var controllerType in controllerTypes)
        {
            var methods = controllerType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(IsActionMethod)
                .ToList();
            permisoDto.AddRange(from method in methods let allowAnonymousAttribute = method.GetCustomAttribute<AllowAnonymousAttribute>() let methodName = method.Name where allowAnonymousAttribute == null select new CrearPermisoDto(0, methodName, "permiso", controllerType.Name));
        }

        return permisoDto;
    }

    private List<CrearPermisoDto> ListMenuPermissions()
    {
        var listaMenus = new List<CrearPermisoDto>();
        try
        {
            var r = new  StreamReader("Utilities/vistas.json");
            var json = r.ReadToEnd();
            var jsonObject = JsonSerializer.Deserialize<string[]>(json);
            if (jsonObject != null)
                listaMenus.AddRange(jsonObject.Select(menu => new CrearPermisoDto(0, menu, "vista", string.Empty)));
        }
        catch (Exception e)
        {
            listaMenus = new List<CrearPermisoDto>();
        }
        return listaMenus;
    }
    private static bool IsActionMethod(MethodInfo method)
    {
        return typeof(IActionResult).IsAssignableFrom(method.ReturnType) ||
               (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>));
    }
    [HttpGet("getall")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PermisoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(new ObtenerPermisosQuery(), ct);
        return Ok(result);
    }
    [HttpGet("getbyrol/{rolId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PermisoRolDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByRol(int rolId, CancellationToken ct) {
        var result = await _dispatcher.QueryAsync(new ObtenerPermisosRolPorRolIdQuery(rolId), ct);
        return Ok(result);
    }

    // POST api/permiso
    [HttpPost]
    [ProducesResponseType(typeof(PermisoRolDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CrearPermisoRolDto dto, CancellationToken ct) {
        var command = new CrearPermisoRolCommand(
            dto.PermisoId,dto.RolId);

        var result = await _dispatcher.SendAsync(command, ct);

        // La URL de retorno usa el PublicId (Guid), no el int
        return Created();
    }
    // DELETE api/permiso/1/2
    [HttpDelete("{permisoId:int}/{rolId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int permisoId, int rolId, CancellationToken ct) {
        await _dispatcher.SendAsync(new EliminarPermisoRolCommand(permisoId,rolId), ct);
        return NoContent();
    }
}