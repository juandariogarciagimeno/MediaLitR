using MediaLitr.Abstractions.CQRS;
using MediaLitr.Abstractions.Pipelines;
using Microsoft.Extensions.Logging;

namespace MediaLitr.Console.Features.Behaviors
{
    public class LogginBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
    {
        private ILogger<LogginBehavior<TRequest, TResponse>> logger;
        public LogginBehavior(ILogger<LogginBehavior<TRequest, TResponse>> logger) 
        {
            this.logger = logger;
        }
        public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken, RequestDelegate<TResponse> next)
        {
            logger.LogInformation("Handling request of type {RequestType} with data: {@RequestData}", typeof(TRequest).Name, request);
            var r = await next();
            logger.LogInformation("Finished handling request of type {RequestType}", typeof(TRequest).Name);
            return r;
        }
    }
}
