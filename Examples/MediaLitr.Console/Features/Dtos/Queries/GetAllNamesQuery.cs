using MediaLitr.Abstractions.CQRS;

namespace MediaLitr.Console.Features.Dtos.Queries;

public class GetAllNamesQuery : IQuery<GetAllNamesResponse>
{
}


public class GetAllNamesResponse
{
    public List<string> Names { get; set; } = new();
}