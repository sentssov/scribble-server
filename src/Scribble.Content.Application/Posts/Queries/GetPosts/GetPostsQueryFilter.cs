using Scribble.Content.Application.Abstractions;

namespace Scribble.Content.Application.Posts.Queries.GetPosts;

public class GetPostsQueryFilter : GetQueryFilter
{
    public Guid? GroupId { get; set; }
}