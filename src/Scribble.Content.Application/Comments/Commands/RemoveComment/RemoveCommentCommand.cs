using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Comments.Commands.RemoveComment;

public class RemoveCommentCommand : ICommand
{
    public RemoveCommentCommand(CommentId commentId) =>
        CommentId = commentId;

    public CommentId CommentId { get; }
}