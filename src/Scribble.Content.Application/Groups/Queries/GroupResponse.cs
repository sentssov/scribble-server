using System.Collections.ObjectModel;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Groups.Queries;

public class GroupResponse
{
    public GroupId Id { get; set; }
    public UserId UserId { get; set; }
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime ModifiedOnUtc { get; set; }
    public Collection<Link> Links { get; set; } = new();
}