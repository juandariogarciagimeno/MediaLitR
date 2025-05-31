// --------------------------------------------------------------------------------------------------
// <copyright file="IQueryHandler.cs" company="juandariogg">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace MediaLitr.Abstractions.CQRS;

/// <summary>
/// Query handler interface with input and return parameters for CQRS pattern.
/// </summary>
/// <typeparam name="TQuery">Query Type.</typeparam>
/// <typeparam name="TResult">Return Type.</typeparam>
public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
