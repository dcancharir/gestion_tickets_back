using API.Extensions;
using Application.Ports.Driven;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace API.Middlewares;

public class RolePermissionMiddleware
{
    private readonly RequestDelegate _next;

    public RolePermissionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IPermisoRolRepository permisoRolRepo)
    {
        var endpoint = context.GetEndpoint();

        // Sin endpoint mapeado → dejar pasar (404 lo maneja el runtime)
        if (endpoint is null)
        {
            await _next(context);
            return;
        }

        // Rutas marcadas con [AllowAnonymous] → sin verificación
        if (endpoint.Metadata.GetMetadata<IAllowAnonymous>() is not null)
        {
            await _next(context);
            return;
        }

        // Usuario no autenticado → 401 y cortar pipeline
        if (context.User.Identity?.IsAuthenticated != true)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { mensaje = "No autenticado." });
            return;                                          // ← return que faltaba
        }

        // Usuarios con acceso total omiten la verificación de permisos por rol
        if (context.User.GetHasFullAccess())
        {
            await _next(context);
            return;
        }

        // Solo aplica a acciones de controladores MVC/API
        var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (actionDescriptor is null)
        {
            await _next(context);
            return;
        }

        var rolId       = context.User.GetRolId();
        var controlador = actionDescriptor.ControllerName;
        var metodo      = actionDescriptor.ActionName;

        var tienePermiso = await permisoRolRepo
            .VerificarSiRolTienePermisoDeControladorAsync(rolId, metodo, controlador);

        if (!tienePermiso)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new
            {
                mensaje = $"Tu rol no tiene permiso para ejecutar '{metodo}' en '{controlador}'."
            });
            return;
        }

        await _next(context);
    }
}
