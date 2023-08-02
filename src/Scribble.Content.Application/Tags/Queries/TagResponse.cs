using System.Collections.ObjectModel;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Tags.Queries;

public class TagResponse
{
    public TagId Id { get; set; }
    public UserId UserId { get; set; }
    public string Name { get; set; }
    public Collection<Link> Links { get; set; } = new();
}