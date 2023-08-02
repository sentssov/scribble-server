using FluentValidation;
using Scribble.Content.Application.Extensions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.UserId)
            .NotEqual(UserId.Empty);
        
        RuleFor(x => x.Name)
            .MustBeValueObject(async v =>
            {
                var isNameUnique = await repository
                    .UniqueByAsync<Category, CategoryId>(x => x.Name.Value != v)
                    .ConfigureAwait(false);

                return CategoryName.Create(v, isNameUnique);
            });
    }
}