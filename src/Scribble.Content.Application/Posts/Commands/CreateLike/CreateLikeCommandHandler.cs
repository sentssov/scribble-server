using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Posts.Commands.CreateLike;

internal class CreateLikeCommandHandler : ICommandHandler<CreateLikeCommand>
{
    private readonly IEntityRepository _repository;

    public CreateLikeCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result> Handle(CreateLikeCommand command, CancellationToken token)
    {
        var postResult = Result.Create(await _repository
            .GetAsync<Post, PostId>(command.PostId, token));

        return postResult
            .Bind(p => p.AddLike(command.UserId))
            .Map(l => l.Id);
    }
}