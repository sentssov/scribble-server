using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQuery : IQuery<CategoryResponse>
{
    public GetCategoryByIdQuery(CategoryId categoryId) => 
        CategoryId = categoryId;
    
    public CategoryId CategoryId { get; init; }
}