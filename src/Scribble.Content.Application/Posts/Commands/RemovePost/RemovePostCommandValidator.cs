using FluentValidation;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Application.Posts.Commands.RemovePost;

public class RemovePostCommandValidator : AbstractValidator<RemovePostCommand>
{
    public RemovePostCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.PostId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Post, PostId>(id, token))
            .WithMessage("The post with specified identifier does not exists");
    }
}