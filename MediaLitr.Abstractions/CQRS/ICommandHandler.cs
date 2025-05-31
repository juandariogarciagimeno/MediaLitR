// --------------------------------------------------------------------------------------------------
// <copyright file="ICommandHandler.cs" company="juandariogg">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace MediaLitr.Abstractions.CQRS;

using MediaLitr.Abstractions.Models;

/// <summary>
/// Command handler interface with input and return parameters for CQRS pattern.
/// </summary>
/// <typeparam name="TCommand">Command Type.</typeparam>
/// <typeparam name="TResult">Return Type.</typeparam>
public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Command handler interface without return value for CQRS pattern.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand<Unit>
{
}
