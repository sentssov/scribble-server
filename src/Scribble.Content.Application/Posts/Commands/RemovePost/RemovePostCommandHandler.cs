using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Posts.Commands.RemovePost;

internal class RemovePostCommandHandler : ICommandHandler<RemovePostCommand>
{
    private readonly IEntityRepository _repository;

    public RemovePostCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result> Handle(RemovePostCommand command, CancellationToken token)
    {
        await _repository.RemoveAsync<Post, PostId>(command.PostId, token)
            .ConfigureAwait(false);

        return Result.Success();
    }
}