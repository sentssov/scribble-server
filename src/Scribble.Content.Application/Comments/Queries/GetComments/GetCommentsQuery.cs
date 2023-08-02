using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Comments.Queries.GetComments;

public class GetCommentsQuery : IQuery<IPagedCollection<CommentResponse>>
{
    public GetCommentsQuery(PostId postId, GetQueryPagination pagination, GetCommentsQueryFilter filter)
    {
        PostId = postId;
        Pagination = pagination;
        Filter = filter;
    }

    public PostId PostId { get; }
    public GetQueryPagination Pagination { get; }
    public GetCommentsQueryFilter Filter { get; }
}