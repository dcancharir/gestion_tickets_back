using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
namespace Application.CQRS.Core;

public static class CqrsServiceExtensions {
    /// <summary>
    /// Registra el Dispatcher y escanea el ensamblado actual para registrar
    /// automáticamente todos los ICommandHandler e IQueryHandler encontrados.
    /// Uso en Program.cs: builder.Services.AddCqrs();
    /// </summary>
    public static IServiceCollection AddCqrs(this IServiceCollection services) {
        // Registrar el Dispatcher
        services.AddScoped<IDispatcher, Dispatcher>();

        // Escanear todos los tipos del ensamblado actual
        var assembly = Assembly.GetExecutingAssembly();

        RegisterHandlers(services, assembly, typeof(ICommandHandler<,>));
        RegisterHandlers(services, assembly, typeof(IQueryHandler<,>));

        return services;
    }

    private static void RegisterHandlers(
        IServiceCollection services,
        Assembly assembly,
        Type openGenericInterface) {
        var handlerTypes = assembly
            .GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false }
                     && t.GetInterfaces().Any(i =>
                            i.IsGenericType &&
                            i.GetGenericTypeDefinition() == openGenericInterface));

        foreach(var handlerType in handlerTypes) {
            var interfaceType = handlerType
                .GetInterfaces()
                .First(i => i.IsGenericType &&
                            i.GetGenericTypeDefinition() == openGenericInterface);

            services.AddScoped(interfaceType, handlerType);
        }
    }
}
