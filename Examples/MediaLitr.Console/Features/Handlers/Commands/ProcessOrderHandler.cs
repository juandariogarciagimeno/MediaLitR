using MediaLitr.Abstractions.CQRS;
using MediaLitr.Console.Features.Dtos.Commands;

namespace MediaLitr.Console.Features.Handlers.Commands;

public class ProcessOrderHandler : ICommandHandler<ProcessOrderCommand, ProcessOrderResponse>
{
    public Task<ProcessOrderResponse> HandleAsync(ProcessOrderCommand command, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new ProcessOrderResponse
        {
            IsSuccess = true,
            Message = $"Order {command.OrderId} has been processed successfully."
        });
    }
}