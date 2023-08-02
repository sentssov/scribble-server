using FluentValidation;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Application.Posts.Commands.RemoveTag;

public class RemoveTagCommandValidator : AbstractValidator<RemoveTagCommand>
{
    public RemoveTagCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.PostId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Post, PostId>(id, token))
            .WithMessage("The post with specified identifier does not exists");
        
        RuleFor(x => x.TagId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Tag, TagId>(id, token))
            .WithMessage("The tag with specified identifier does not exists");
    }
}