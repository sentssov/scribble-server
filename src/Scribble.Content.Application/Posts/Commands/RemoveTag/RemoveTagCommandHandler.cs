using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Posts.Commands.RemoveTag;

internal class RemoveTagCommandHandler : ICommandHandler<RemoveTagCommand>
{
    private readonly IEntityRepository _repository;

    public RemoveTagCommandHandler(IEntityRepository repository) =>
        _repository = repository;

    public async Task<Result> Handle(RemoveTagCommand command, CancellationToken token)
    {
        var postResult = Result.Create(await _repository
            .GetAsync<Post, PostId>(command.PostId, token));
        var tagResult = Result.Create(await _repository
            .GetAsync<Tag, TagId>(command.TagId, token));
        
        return Result.Combine(postResult, tagResult)
            .Bind(() => postResult.Value.RemoveTag(tagResult.Value.Id));
    }
}