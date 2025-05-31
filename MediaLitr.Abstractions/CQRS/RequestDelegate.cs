// --------------------------------------------------------------------------------------------------
// <copyright file="RequestDelegate.cs" company="juandariogg">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

namespace MediaLitr.Abstractions.CQRS;

/// <summary>
/// Request delegate for handling asynchronous requests in pipelines in CQRS pattern.
/// </summary>
/// <typeparam name="TResponse">Response Type.</typeparam>
/// <param name="t">Cancellation Token.</param>
/// <returns>A <see cref="Task"/> of type <typeparamref name="TResponse"/>.</returns>
public delegate Task<TResponse> RequestDelegate<TResponse>(CancellationToken t = default);
