using Scribble.Content.Application.Abstractions;

namespace Scribble.Content.Application.Comments.Queries.GetComments;

public class GetCommentsQueryFilter : GetQueryFilter
{
    public string? CategoryName { get; set; }
}