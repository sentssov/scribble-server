using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Posts.Commands.RemovePost;

public class RemovePostCommand : ICommand
{
    public RemovePostCommand(PostId postId) =>
        PostId = postId;

    public PostId PostId { get; }
}