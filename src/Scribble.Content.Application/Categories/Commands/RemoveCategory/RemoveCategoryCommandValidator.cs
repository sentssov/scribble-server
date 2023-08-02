using FluentValidation;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Categories.Commands.RemoveCategory;

public class RemoveCategoryCommandValidator : AbstractValidator<RemoveCategoryCommand>
{
    public RemoveCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEqual(CategoryId.Empty);
    }
}