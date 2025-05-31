using MediaLitr.Abstractions.Models;

namespace MediaLitr.Abstractions.CQRS;

public interface ICommand<TRes>
{
}

public interface ICommand : ICommand<Unit>
{
}