using Scribble.Content.Application.Abstractions;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Groups.Queries.GetGroups;

public class GetGroupsQueryFilter : GetQueryFilter
{
    public Guid? UserId { get; set; }
    public string? CategoryName { get; set; }
}