using FluentValidation;
using Scribble.Content.Application.Extensions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Posts.Commands.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.Title)
            .MustBeValueObject(PostTitle.Create);

        RuleFor(x => x.Description)
            .MustBeValueObject(PostDescription.Create);

        RuleFor(x => x.HtmlContent)
            .MustBeValueObject(PostContent.Create);
        
        RuleFor(x => x.GroupId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Group, GroupId>(id, token))
            .WithMessage("The group with specified identifier does not exists.");
    }
}