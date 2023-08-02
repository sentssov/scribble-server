using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Groups.Queries.GetGroups;

public class GetGroupsQuery : IQuery<IPagedCollection<GroupResponse>>
{
    public GetGroupsQuery(GetQueryPagination pagination, GetGroupsQueryFilter filter)
    {
        Pagination = pagination;
        Filter = filter;
    }

    public GetQueryPagination Pagination { get; }
    public GetGroupsQueryFilter Filter { get; }
}