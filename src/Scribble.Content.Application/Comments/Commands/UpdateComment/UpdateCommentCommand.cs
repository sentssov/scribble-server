using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Comments.Commands.UpdateComment;

public class UpdateCommentCommand : ICommand
{
    public UpdateCommentCommand(CommentId commentId, string text)
    {
        CommentId = commentId;
        Text = text;
    }
    
    public CommentId CommentId { get; }
    public string Text { get; }
}