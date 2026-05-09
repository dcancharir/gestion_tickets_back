using API.Extensions;
using Application.Ports.Driven;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace API.Middlewares;

public class RolePermissionMiddleware {
    private readonly RequestDelegate _next;
    public RolePermissionMiddleware(RequestDelegate next) {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context, IPermisoRolRepository _permisoRolRepository) {
        var endpoint = context.GetEndpoint();
        if(endpoint == null) {
            await _next(context);
            return;
        }
        //Permitir AllowAnonymous
        var allowAnonymous = endpoint.Metadata.GetMetadata<IAllowAnonymous>();

        if(allowAnonymous != null) {
            await _next(context);
            return;
        }
        if(context.User.Identity?.IsAuthenticated != true) {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(new {
                mensaje = "No autenticado"
            });

         
        }
        var actionDescriptor = endpoint.Metadata
         .GetMetadata<ControllerActionDescriptor>();

        // No es un controlador MVC
        if(actionDescriptor == null) {
            await _next(context);
            return;
        }
        var rolId = context.User.GetRolId();

        var controlador = actionDescriptor.ControllerName;
        var metodo = actionDescriptor.ActionName;

        var tienePermiso = await _permisoRolRepository
            .VerificarSiRolTienePermisoDeControladorAsync(
                rolId,
                metodo,
                controlador
                );

        if(!tienePermiso) {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            await context.Response.WriteAsJsonAsync(new {
                mensaje = "No tiene permisos para acceder a este recurso"
            });

            return;
        }
        await _next(context);
    }
}
