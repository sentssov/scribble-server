using System.Collections.ObjectModel;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Comments.Queries;

public class CommentResponse
{
    public CommentId Id { get; set; }
    public string Text { get; set; }
    public UserId UserId { get; set; }
    public PostId PostId { get; set; }
    public Collection<Link> Links { get; set; } = new();
}