using FluentValidation;
using Scribble.Content.Application.Extensions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Groups.Commands.CreateGroup;

public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.Name)
            .MustBeValueObject(async name =>
            {
                var isNameUnique = await repository
                    .UniqueByAsync<Group, GroupId>(x => x.Name.Value != name)
                    .ConfigureAwait(false);

                return GroupName.Create(name, isNameUnique);
            });

        RuleFor(x => x.ShortName)
            .MustBeValueObject(async shortName =>
            {
                var isShortNameUnique = await repository
                    .UniqueByAsync<Group, GroupId>(x => x.ShortName.Value != shortName)
                    .ConfigureAwait(false);

                return GroupShortName.Create(shortName, isShortNameUnique);
            });

        RuleFor(x => x.Description)
            .MustBeValueObject(GroupDescription.Create);

        RuleFor(x => x.UserId)
            .NotEqual(UserId.Empty);
    }
}