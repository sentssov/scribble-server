using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Comments.Commands.CreateComment;

internal class CreateCommentCommandHandler : ICommandHandler<CreateCommentCommand, CommentId>
{
    private readonly IEntityRepository _repository;

    public CreateCommentCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result<CommentId>> Handle(CreateCommentCommand command, CancellationToken token)
    {
        var commentText = CommentText.Create(command.Text).Value;

        var postResult = Result.Create(await _repository
            .GetAsync<Post, PostId>(command.PostId, token));

        return postResult
            .Bind(p => p.CreateComment(commentText, command.UserId))
            .Map(c => c.Id);
    }
}