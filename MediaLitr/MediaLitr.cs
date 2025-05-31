// --------------------------------------------------------------------------------------------------
// <copyright file="MediaLitR.cs" company="juandariogg">
// Licensed under the MIT license. See LICENSE file in the samples root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------

using MediaLitr.Abstractions;
using MediaLitr.Abstractions.CQRS;
using MediaLitr.Abstractions.Models;
using MediaLitr.Abstractions.Pipelines;
using MediaLitr.Config;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MediaLitr.Test")]

namespace MediaLitr;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/// <summary>
/// MediaLitr is a mediator implementation that allows for the execution of commands and queries.
/// </summary>
/// <param name="serviceProvider">Service Provider.</param>
/// <param name="pipelines">Registered Generic Pipelines.</param>
internal class MediaLitR(IServiceProvider serviceProvider, IOptions<PipelineConfig> pipelines) : IMediator
{
    private readonly IServiceProvider serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    private readonly PipelineConfig pipelines = pipelines?.Value ?? throw new ArgumentNullException(nameof(pipelines));

    /// <inheritdoc/>
    public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>
    {
        var handler = serviceProvider.GetService<IQueryHandler<TQuery, TResult>>();

        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for query type {typeof(TQuery).Name}");
        }

        var final = GetPipeline<TQuery, TResult>(handler.HandleAsync);

        return await final.Invoke(query, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResult>
    {
        var handler = serviceProvider.GetService<ICommandHandler<TCommand, TResult>>();

        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for command type {typeof(TCommand).Name}");
        }

        var final = GetPipeline<TCommand, TResult>(handler.HandleAsync);

        return await final.Invoke(command, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<Unit>
    {
        var handler = serviceProvider.GetService<ICommandHandler<TCommand>>();

        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for command type {typeof(TCommand).Name}");
        }

        var final = GetPipeline<TCommand, Unit>(handler.HandleAsync);

        await final.Invoke(command, cancellationToken);
    }

    private Func<TRequest, CancellationToken, Task<TResponse>> GetPipeline<TRequest, TResponse>(Func<TRequest, CancellationToken, Task<TResponse>> handler)
        where TRequest : ICommand<TResponse>
    {
        List<IPipelineBehavior<TRequest, TResponse>>? allPipelines = [];

        var specificPipelines = serviceProvider.GetServices<IPipelineBehavior<TRequest, TResponse>>();

        if (pipelines.GenericPipelines?.Any() ?? false)
        {
            allPipelines.AddRange(pipelines.GenericPipelines.Select(x =>
            {
                var pl = x.MakeGenericType(typeof(TRequest), typeof(TResponse));
                var instance = ActivatorUtilities.CreateInstance(serviceProvider, pl);
                return (IPipelineBehavior<TRequest, TResponse>)instance;
            }));
        }

        if (specificPipelines.Any())
        {
            allPipelines.AddRange(specificPipelines);
        }

        var final = BuildPipeline(allPipelines, handler);
        return final;
    }

    private static Func<TRequest, CancellationToken, Task<TResponse>> BuildPipeline<TRequest, TResponse>(IEnumerable<IPipelineBehavior<TRequest, TResponse>> behaviors, Func<TRequest, CancellationToken, Task<TResponse>> finalHandler)
        where TRequest : ICommand<TResponse>
    {
        Func<TRequest, CancellationToken, Task<TResponse>> pipeline = finalHandler;

        foreach (var behavior in behaviors.Reverse())
        {
            var next = pipeline;
            pipeline = (request, ct) => behavior.HandleAsync(request, ct, (ct) => next(request, ct));
        }

        return pipeline;
    }
}
