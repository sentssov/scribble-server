using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Comments.Commands.CreateComment;

public class CreateCommentCommand : ICommand<CommentId>
{
    public CreateCommentCommand(PostId postId, UserId userId, string text)
    {
        PostId = postId;
        UserId = userId;
        Text = text;
    }

    public PostId PostId { get; }
    public UserId UserId { get; }
    public string Text { get; }
}