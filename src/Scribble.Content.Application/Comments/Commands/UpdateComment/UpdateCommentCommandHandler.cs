using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Comments.Commands.UpdateComment;

internal class UpdateCommentCommandHandler : ICommandHandler<UpdateCommentCommand>
{
    private readonly IEntityRepository _repository;

    public UpdateCommentCommandHandler(IEntityRepository repository) =>
        _repository = repository;

    public async Task<Result> Handle(UpdateCommentCommand command, CancellationToken token)
    {
        var commentText = CommentText.Create(command.Text).Value;
        
        var commentResult = Result.Create(await _repository
            .GetAsync<Comment, CommentId>(command.CommentId, token));

        return commentResult.Bind(c => c.Update(commentText));
    }
}