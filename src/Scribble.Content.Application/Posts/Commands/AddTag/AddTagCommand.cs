using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Posts.Commands.AddTag;

public class AddTagCommand : ICommand
{
    public AddTagCommand(PostId postId, TagId tagId)
    {
        PostId = postId;
        TagId = tagId;
    }
    
    public PostId PostId { get; }
    public TagId TagId { get; }
}