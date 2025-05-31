using MediaLitr.Abstractions.CQRS;

namespace MediaLitr.Console.Features.Dtos.Commands
{
    public class SendConfirmationMailCommand : ICommand
    {
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
