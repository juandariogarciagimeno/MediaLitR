namespace MediaLitr.Abstractions.CQRS;

public interface IQuery<TRes> : ICommand<TRes>
{
}
