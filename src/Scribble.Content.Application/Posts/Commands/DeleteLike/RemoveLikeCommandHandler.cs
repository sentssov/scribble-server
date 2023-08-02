using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Posts.Commands.DeleteLike;

internal class RemoveLikeCommandHandler : ICommandHandler<RemoveLikeCommand>
{
    private readonly IEntityRepository _repository;

    public RemoveLikeCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result> Handle(RemoveLikeCommand command, CancellationToken token)
    {
        var postResult = Result.Create(await _repository
            .GetAsync<Post, PostId>(command.PostId, token));

        return postResult.Bind(p => p.RemoveLike(command.UserId));
    }
}