using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Tags.Commands.RemoveTag;

internal class RemoveTagCommandHandler : ICommandHandler<RemoveTagCommand>
{
    private readonly IEntityRepository _repository;

    public RemoveTagCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result> Handle(RemoveTagCommand command, CancellationToken token)
    {
        await _repository.RemoveAsync<Tag, TagId>(command.TagId, token)
            .ConfigureAwait(false);

        return Result.Success();
    }
}