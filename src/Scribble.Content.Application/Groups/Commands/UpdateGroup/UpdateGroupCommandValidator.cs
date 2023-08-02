using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scribble.Content.Application.Extensions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Groups.Commands.UpdateGroup;

public class UpdateGroupCommandValidator : AbstractValidator<UpdateGroupCommand>
{
    public UpdateGroupCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.GroupId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Group, GroupId>(id, token))
            .WithMessage("The group with specified identifier does not exists.");
        
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
    }
}