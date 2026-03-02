using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Core;

/// <summary>
/// Implementación del Dispatcher que resuelve los handlers
/// desde el contenedor de inyección de dependencias de ASP.NET Core.
/// </summary>
public class Dispatcher : IDispatcher {
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> SendAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default) {
        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(command.GetType(), typeof(TResult));

        var handler = _serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException(
                $"No se encontró un handler registrado para el comando '{command.GetType().Name}'. " +
                $"Asegúrate de registrar '{handlerType.Name}' en Program.cs.");

        // Invocación mediante reflexión para mantener la genericidad
        var method = handlerType.GetMethod(nameof(ICommandHandler<ICommand<TResult>, TResult>.HandleAsync))!;
        var task = (Task<TResult>)method.Invoke(handler, new object[] { command, cancellationToken })!;

        return await task;
    }

    public async Task<TResult> QueryAsync<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default) {
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResult));

        var handler = _serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException(
                $"No se encontró un handler registrado para la consulta '{query.GetType().Name}'. " +
                $"Asegúrate de registrar '{handlerType.Name}' en Program.cs.");

        var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))!;
        var task = (Task<TResult>)method.Invoke(handler, new object[] { query, cancellationToken })!;

        return await task;
    }
}