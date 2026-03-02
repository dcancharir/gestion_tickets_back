using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.Core;

// ── Marcadores base ──────────────────────────────────────────────────────────

/// <summary>Comando que devuelve un resultado de tipo TResult.</summary>
public interface ICommand<TResult> { }

/// <summary>Comando que no devuelve resultado (fire-and-forget).</summary>
public interface ICommand : ICommand<Unit> { }

/// <summary>Consulta que devuelve un resultado de tipo TResult.</summary>
public interface IQuery<TResult> { }

// ── Handlers ─────────────────────────────────────────────────────────────────

/// <summary>Handler para un comando que devuelve TResult.</summary>
public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult> {
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>Handler para un comando sin resultado.</summary>
public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand<Unit> { }

/// <summary>Handler para una consulta.</summary>
public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult> {
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}

// ── Dispatcher ───────────────────────────────────────────────────────────────

/// <summary>Punto central que despacha Commands y Queries a sus handlers.</summary>
public interface IDispatcher {
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}

// ── Tipo Unit (equivalente a void en async) ───────────────────────────────────

/// <summary>Representa ausencia de valor de retorno en comandos sin resultado.</summary>
public readonly struct Unit {
    public static readonly Unit Value = new();
}
