using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Posts.Commands.CreateLike;

public class CreateLikeCommand : ICommand
{
    public CreateLikeCommand(PostId postId, UserId userId)
    {
        PostId = postId;
        UserId = userId;
    }

    public PostId PostId { get; }
    public UserId UserId { get; }
}