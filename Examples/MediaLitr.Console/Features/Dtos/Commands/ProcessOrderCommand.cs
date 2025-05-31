using MediaLitr.Abstractions.CQRS;

namespace MediaLitr.Console.Features.Dtos.Commands;

public class ProcessOrderCommand : ICommand<ProcessOrderResponse>
{
    public string OrderId { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class ProcessOrderResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}
