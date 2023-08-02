using System.Collections.ObjectModel;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Categories.Queries;

public class CategoryResponse
{
    public CategoryId Id { get; init; }
    public string Name { get; set; }
    public UserId UserId { get; set; }
    public Collection<Link> Links { get; set; } = new();
}