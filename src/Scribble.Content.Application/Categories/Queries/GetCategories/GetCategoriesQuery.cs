using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;

namespace Scribble.Content.Application.Categories.Queries.GetCategories;

public class GetCategoriesQuery : IQuery<IPagedCollection<CategoryResponse>>
{
    public GetCategoriesQuery(GetQueryPagination pagination, GetCategoriesQueryFilter filter)
    {
        Pagination = pagination;
        Filter = filter;
    }

    public GetQueryPagination Pagination { get; }
    public GetCategoriesQueryFilter Filter { get; }
}