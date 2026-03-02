using Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace API.Middlewares;

public class ExceptionMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        } catch(Exception ex) {
            _logger.LogError(ex, "Error no controlado: {Message}", ex.Message);
            await ManejarExcepcionAsync(context, ex);
        }
    }

    private static Task ManejarExcepcionAsync(HttpContext context, Exception ex) {
        var (statusCode, mensaje) = ex switch {
            NotFoundException e => (HttpStatusCode.NotFound, e.Message),
            ValidationException e => (HttpStatusCode.BadRequest, e.Message),
            ConflictException e => (HttpStatusCode.Conflict, e.Message),
            _ => (HttpStatusCode.InternalServerError,
                                       "Ocurrió un error interno. Intente nuevamente.")
        };

        var respuesta = JsonSerializer.Serialize(new {
            status = (int)statusCode,
            mensaje
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(respuesta);
    }
}