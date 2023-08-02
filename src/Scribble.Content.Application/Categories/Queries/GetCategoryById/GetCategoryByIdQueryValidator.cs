using FluentValidation;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator(IEntityRepository repository)
    {
        RuleFor(x => x.CategoryId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Category, CategoryId>(id, token))
            .WithMessage("The category with specified identifier does not exists.");
    }
}