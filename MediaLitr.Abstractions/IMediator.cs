// --------------------------------------------------------------------------------------------------
// <copyright file="IMediator.cs" company="juandariogg">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace MediaLitr.Abstractions;

using MediaLitr.Abstractions.CQRS;
using MediaLitr.Abstractions.Models;

/// <summary>
/// Mediator interface for handling commands and queries in a CQRS pattern.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends a command and returns a result.
    /// </summary>
    /// <typeparam name="TCommand">Command Type.</typeparam>
    /// <typeparam name="TResult">Return Type.</typeparam>
    /// <param name="command">Command instance.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A <see cref="Task{TResult}"/>.</returns>
    Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResult>;

    /// <summary>
    /// Sends a command without a return value.
    /// </summary>
    /// <typeparam name="TCommand">Command Type.</typeparam>
    /// <param name="command">Command Instance.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A <see cref="Task"/>.</returns>
    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<Unit>;

    /// <summary>
    /// Queries for a result based on the provided query object.
    /// </summary>
    /// <typeparam name="TQuery">Query Type.</typeparam>
    /// <typeparam name="TResult">Result Type.</typeparam>
    /// <param name="query">Query Instance.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>a <see cref="Task{TResult}"/>.</returns>
    Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>;
}
