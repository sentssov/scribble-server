using FluentValidation;
using Scribble.Content.Application.Extensions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Tags.Commands.CreateTag;

public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.Name)
            .MustBeValueObject(async name =>
            {
                var isNameUnique = await repository
                    .UniqueByAsync<Tag, TagId>(x => x.Name.Value != name)
                    .ConfigureAwait(false);

                return TagName.Create(name, isNameUnique);
            });
        
        RuleFor(x => x.UserId)
            .NotEqual(UserId.Empty);
    }
}