using FluentValidation;
using Scribble.Content.Application.Extensions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.CategoryId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Category, CategoryId>(id, token))
            .WithMessage("The category with specified identifier does not exists.");

        RuleFor(x => x.Name)
            .MustBeValueObject(async value =>
            {
                var isNameUnique = await repository
                    .UniqueByAsync<Category, CategoryId>(x => x.Name.Value != value)
                    .ConfigureAwait(false);

                return CategoryName.Create(value, isNameUnique);
            });
    }
}