using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Posts.Queries.GetPosts;

public class GetPostsQuery : IQuery<IPagedCollection<PostResponse>>
{
    public GetPostsQuery(GroupId groupId, GetQueryPagination pagination, GetPostsQueryFilter filter)
    {
        GroupId = groupId;
        Pagination = pagination;
        Filter = filter;
    }

    public GroupId GroupId { get; }
    public GetQueryPagination Pagination { get; }
    public GetPostsQueryFilter Filter { get; }
}