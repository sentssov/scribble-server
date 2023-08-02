using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Groups.Commands.RemoveGroup;

internal class RemoveGroupCommandHandler : ICommandHandler<RemoveGroupCommand>
{
    private readonly IEntityRepository _repository;

    public RemoveGroupCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result> Handle(RemoveGroupCommand command, CancellationToken token)
    {
        await _repository.RemoveAsync<Group, GroupId>(command.GroupId, token)
            .ConfigureAwait(false);

        return Result.Success();
    }
}