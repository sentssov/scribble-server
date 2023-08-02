using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Posts.Commands.UpdatePost;

internal class UpdatePostCommandHandler : ICommandHandler<UpdatePostCommand>
{
    private readonly IEntityRepository _repository;

    public UpdatePostCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result> Handle(UpdatePostCommand command, CancellationToken token)
    {
        var postTitle = PostTitle.Create(command.Title).Value;
        var postContent = PostContent.Create(command.Content).Value;
        var postDescription = PostDescription.Create(command.Description).Value;

        var postResult = Result.Create(await _repository
            .GetAsync<Post, PostId>(command.PostId, token));

        return postResult.Bind(p => p.Update(postTitle, postContent, postDescription));
    }
}