using Scribble.Content.Application.Abstractions;

namespace Scribble.Content.Application.Categories.Queries.GetCategories;

public class GetCategoriesQueryFilter : GetQueryFilter
{
    public Guid? AuthorId { get; set; }
}