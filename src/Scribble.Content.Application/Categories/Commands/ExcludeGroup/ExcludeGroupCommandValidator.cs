using FluentValidation;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Application.Categories.Commands.ExcludeGroup;

public class ExcludeGroupCommandValidator : AbstractValidator<ExcludeGroupCommand>
{
    public ExcludeGroupCommandValidator(IEntityRepository repository)
    {
        
    }
}