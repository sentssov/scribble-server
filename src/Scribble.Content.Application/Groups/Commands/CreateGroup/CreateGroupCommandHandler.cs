using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Groups.Commands.CreateGroup;

internal class CreateGroupCommandHandler : ICommandHandler<CreateGroupCommand, GroupId>
{
    private readonly IEntityRepository _repository;
    public CreateGroupCommandHandler(IEntityRepository repository) =>
        _repository = repository;

    public async Task<Result<GroupId>> Handle(CreateGroupCommand command, CancellationToken token)
    {
        var isNameUnique = await _repository
            .UniqueByAsync<Group, GroupId>(x => x.Name.Value != command.Name, token)
            .ConfigureAwait(false);
        
        var isShortNameUnique = await _repository
            .UniqueByAsync<Group, GroupId>(x => x.ShortName.Value != command.ShortName, token)
            .ConfigureAwait(false);

        var groupNameResult = GroupName.Create(command.Name, isNameUnique);
        var groupShortNameResult = GroupShortName.Create(command.ShortName, isShortNameUnique);
        var groupDescriptionResult = GroupDescription.Create(command.Description);

        return await Result.Combine(groupNameResult, groupShortNameResult, groupDescriptionResult)
            .Bind(() => Group.Create(
                groupNameResult.Value,
                groupShortNameResult.Value,
                groupDescriptionResult.Value,
                command.UserId))
            .Bind(g => _repository.SaveAsync<Group, GroupId>(g, token));
    }
}