using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Groups.Commands.UpdateGroup;

internal class UpdateGroupCommandHandler : ICommandHandler<UpdateGroupCommand>
{
    private readonly IEntityRepository _repository;

    public UpdateGroupCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result> Handle(UpdateGroupCommand command, CancellationToken token)
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

        var groupResult = Result.Create(await _repository
            .GetAsync<Group, GroupId>(command.GroupId, token));

        return Result.Combine(groupNameResult, groupShortNameResult, groupDescriptionResult, groupResult)
            .Bind(() => groupResult.Value.Update(
                groupNameResult.Value,
                groupShortNameResult.Value,
                groupDescriptionResult.Value));
    }
}