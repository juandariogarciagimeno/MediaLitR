using MediaLitr.Abstractions.CQRS;
using System.Windows.Input;

namespace MediaLitr.Abstractions.Pipelines
{
    public interface IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken, RequestDelegate<TResponse> next);
    }
}
