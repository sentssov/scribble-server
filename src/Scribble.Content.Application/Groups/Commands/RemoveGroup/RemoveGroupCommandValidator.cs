﻿using FluentValidation;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Application.Groups.Commands.RemoveGroup;

public class RemoveGroupCommandValidator : AbstractValidator<RemoveGroupCommand>
{
    public RemoveGroupCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.GroupId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Group, GroupId>(id, token))
            .WithMessage("The group with specified identifier does not exists.");
    }
}