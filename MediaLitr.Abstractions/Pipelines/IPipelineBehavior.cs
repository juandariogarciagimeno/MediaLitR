// --------------------------------------------------------------------------------------------------
// <copyright file="IPipelineBehavior.cs" company="juandariogg">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace MediaLitr.Abstractions.Pipelines;

using MediaLitr.Abstractions.CQRS;

/// <summary>
/// Pipeline behavior interface for commands in a CQRS pattern.
/// </summary>
/// <typeparam name="TRequest">Command/Query Type.</typeparam>
/// <typeparam name="TResponse">Return Type.</typeparam>
public interface IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    /// <summary>
    /// Handles the pipeline and passes the call stack to the next delegate in the pipeline.
    /// </summary>
    /// <param name="request">Query/Command.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="next">Delegate for next item in the pipeline.</param>
    /// <returns>A <see cref="Task"/> of <typeparamref name="TResponse"/>.</returns>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken, RequestDelegate<TResponse> next);
}
