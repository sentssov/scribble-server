using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Tags.Queries.GetTags;

public class GetTagsQuery : IQuery<IPagedCollection<TagResponse>>
{
    public GetTagsQuery(UserId userId, GetQueryPagination pagination, GetTagsQueryFilter filter)
    {
        UserId = userId;
        Pagination = pagination;
        Filter = filter;
    }

    public UserId UserId { get; }
    public GetQueryPagination Pagination { get; }
    public GetTagsQueryFilter Filter { get; }
}