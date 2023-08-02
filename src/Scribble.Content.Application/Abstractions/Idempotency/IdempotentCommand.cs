using Scribble.Content.Application.Abstractions.Messaging;
using StronglyTypedIds;

namespace Scribble.Content.Application.Abstractions.Idempotency;


[StronglyTypedId(StronglyTypedIdBackingType.Guid, StronglyTypedIdConverter.NewtonsoftJson)]
public partial struct RequestId { }

public abstract class IdempotentCommand : ICommand
{
    protected IdempotentCommand(RequestId id) => Id = id;
    public RequestId Id { get; init; }
}

public abstract class IdempotentCommand<TResponse> : ICommand<TResponse>
{
    protected IdempotentCommand(RequestId id) => Id = id;
    public RequestId Id { get; init; }
}