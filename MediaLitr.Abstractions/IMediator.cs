using MediaLitr.Abstractions.CQRS;
using MediaLitr.Abstractions.Models;

namespace MediaLitr.Abstractions
{
    public interface IMediator
    {
        Task<TResult> SendAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand<TResult>;
        Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand<Unit>;
        Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : IQuery<TResult>;
    }
}
