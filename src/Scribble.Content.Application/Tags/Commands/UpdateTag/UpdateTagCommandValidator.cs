using FluentValidation;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Application.Tags.Commands.UpdateTag;

public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    public UpdateTagCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.TagId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Tag, TagId>(id, token))
            .WithMessage("The tag with specified identifier does not exists.");
    }
}