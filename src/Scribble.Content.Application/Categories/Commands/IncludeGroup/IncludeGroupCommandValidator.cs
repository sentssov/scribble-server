using FluentValidation;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Application.Categories.Commands.IncludeGroup;

public class IncludeGroupCommandValidator : AbstractValidator<IncludeGroupCommand>
{
    public IncludeGroupCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.GroupId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Group, GroupId>(id, token))
            .WithMessage("The group with specified identifier does not exists.");
        
        RuleFor(x => x.CategoryId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Category, CategoryId>(id, token))
            .WithMessage("The category with specified identifier does not exists.");
    }
}