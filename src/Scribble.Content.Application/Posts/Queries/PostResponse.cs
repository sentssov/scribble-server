using System.Collections.ObjectModel;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Posts.Queries;

public class PostResponse
{
    public PostId Id { get; set; }
    public GroupId GroupId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime ModifiedOnUtc { get; set; }
    public Collection<Link> Links { get; set; } = new();
}