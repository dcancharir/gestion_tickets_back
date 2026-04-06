using System.Reflection;
using System.Text.Json;
using Application.CQRS.Commands.Permisos;
using Application.CQRS.Core;
using Application.DTOS.Permiso;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PermisoController:ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public PermisoController(IDispatcher dispatcher) => _dispatcher = dispatcher;
    
    [HttpGet]
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
}