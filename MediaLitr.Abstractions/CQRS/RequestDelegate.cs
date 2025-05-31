namespace MediaLitr.Abstractions.CQRS;

public delegate Task<TResponse> RequestDelegate<TResponse>(CancellationToken t = default(CancellationToken));