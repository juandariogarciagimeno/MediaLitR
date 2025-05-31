using MediaLitr.Abstractions.CQRS;
using MediaLitr.Abstractions.Models;
using MediaLitr.Console.Features.Dtos.Commands;

namespace MediaLitr.Console.Features.Handlers.Commands
{
    public class SendConfirmationEmailHandler : ICommandHandler<SendConfirmationMailCommand>
    {
        public Task<Unit> HandleAsync(SendConfirmationMailCommand command, CancellationToken cancellationToken = default)
        {
            return Unit.CompletedTask;
        }
    }
}
