using MediaLitr.Abstractions.CQRS;
using MediaLitr.Console.Features.Dtos.Queries;

namespace MediaLitr.Console.Features.Handlers.Queries
{
    public class GetAllNamesHandler : IQueryHandler<GetAllNamesQuery, GetAllNamesResponse>
    {
        public Task<GetAllNamesResponse> HandleAsync(GetAllNamesQuery query, CancellationToken cancellationToken)
        {
            var response = new GetAllNamesResponse
            {
                Names = new List<string> { "Alice", "Bob", "Charlie" }
            };
            return Task.FromResult(response);
        }
    }
}
