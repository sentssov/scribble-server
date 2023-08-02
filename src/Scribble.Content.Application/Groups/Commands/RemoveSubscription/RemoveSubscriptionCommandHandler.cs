using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Groups.Commands.RemoveSubscription;

internal class RemoveSubscriptionCommandHandler : ICommandHandler<RemoveSubscriptionCommand>
{
    private readonly IEntityRepository _repository;

    public RemoveSubscriptionCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result> Handle(RemoveSubscriptionCommand command, CancellationToken token)
    {
        var groupResult = Result.Create(await _repository
            .GetAsync<Group, GroupId>(command.GroupId, token));

        return groupResult
            .Bind(g => g.RemoveSubscription(command.UserId));
    }
}