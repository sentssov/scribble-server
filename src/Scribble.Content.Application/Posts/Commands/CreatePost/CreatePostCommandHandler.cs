using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Posts.Commands.CreatePost;

internal class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, PostId>
{
    private readonly IEntityRepository _repository;

    public CreatePostCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result<PostId>> Handle(CreatePostCommand command, CancellationToken token)
    {
        var postTitle = PostTitle.Create(command.Title).Value;
        var postContent = PostContent.Create(command.HtmlContent).Value;
        var postDescription = PostDescription.Create(command.Description).Value;

        var groupResult = Result.Create(await _repository
            .GetAsync<Group, GroupId>(command.GroupId, token));

        return groupResult
            .Bind(g => g.CreatePost(postTitle, postContent, postDescription))
            .Map(p => p.Id);
    }
}