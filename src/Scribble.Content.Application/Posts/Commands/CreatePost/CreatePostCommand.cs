using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Posts.Commands.CreatePost;

public class CreatePostCommand : ICommand<PostId>
{
    public CreatePostCommand(GroupId groupId, string title, string htmlContent, string description)
    {
        GroupId = groupId;
        Title = title;
        HtmlContent = htmlContent;
        Description = description;
    }
    
    public GroupId GroupId { get; }
    public string Title { get; }
    public string HtmlContent { get; }
    public string Description { get; }
}