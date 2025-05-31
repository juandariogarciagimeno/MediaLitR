using MediaLitr.Abstractions.CQRS;
using MediaLitr.Abstractions.Pipelines;
using MediaLitr.Console.Features.Dtos.Queries;

namespace MediaLitr.Console.Features.Behaviors
{
    public class GetAllNamesFilterBehavior : IPipelineBehavior<GetAllNamesQuery, GetAllNamesResponse>
    {
        public async Task<GetAllNamesResponse> HandleAsync(GetAllNamesQuery request, CancellationToken cancellationToken, RequestDelegate<GetAllNamesResponse> next)
        {
            return await next();
        }
    }
}
