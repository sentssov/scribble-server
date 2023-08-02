using FluentValidation;
using Scribble.Content.Application.Extensions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Comments.Commands.CreateComment;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator(IEntityRepository repository)
    {
        RuleFor(x => x.PostId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Post, PostId>(id, token))
            .WithMessage("The post with specified identifier does not exists.");

        RuleFor(x => x.Text)
            .MustBeValueObject(CommentText.Create);
        
        RuleFor(x => x.UserId)
            .NotEqual(UserId.Empty);
    }
}
