using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Groups.Commands.CreateSubscription;

internal class CreateSubscriptionCommandHandler : ICommandHandler<CreateSubscriptionCommand>
{
    private readonly IEntityRepository _repository;

    public CreateSubscriptionCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result> Handle(CreateSubscriptionCommand command, CancellationToken token)
    {
        var group = Result.Create(await _repository
            .GetAsync<Group, GroupId>(command.GroupId, token));

        return group.Bind(g => g.CreateSubscription(command.UserId));
    }
}