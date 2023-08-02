using FluentValidation;
using Scribble.Content.Application.Extensions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Posts.Commands.UpdatePost;

public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.PostId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Post, PostId>(id, token))
            .WithMessage("The post with specified identifier does not exists.");

        RuleFor(x => x.Title)
            .MustBeValueObject(PostTitle.Create);

        RuleFor(x => x.Content)
            .MustBeValueObject(PostContent.Create);
        
        RuleFor(x => x.Description)
            .MustBeValueObject(PostDescription.Create);
    }
}