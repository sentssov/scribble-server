using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Posts.Commands.UpdatePost;

public class UpdatePostCommand : ICommand
{
    public UpdatePostCommand(PostId postId, string title, string content, string description)
    {
        PostId = postId;
        Title = title;
        Content = content;
        Description = description;
    }
    
    public PostId PostId { get; }
    public string Title { get; }
    public string Content { get; }
    public string Description { get; }
}