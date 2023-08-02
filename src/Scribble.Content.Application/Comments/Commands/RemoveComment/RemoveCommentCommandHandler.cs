using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Comments.Commands.RemoveComment;

internal class RemoveCommentCommandHandler : ICommandHandler<RemoveCommentCommand>
{
    private readonly IEntityRepository _repository;

    public RemoveCommentCommandHandler(IEntityRepository repository) =>
        _repository = repository;

    public async Task<Result> Handle(RemoveCommentCommand command, CancellationToken token)
    {
        await _repository.RemoveAsync<Comment, CommentId>(command.CommentId, token)
            .ConfigureAwait(false);

        return Result.Success();
    }
}