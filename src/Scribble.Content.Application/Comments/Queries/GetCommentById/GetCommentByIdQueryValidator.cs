using FluentValidation;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Application.Comments.Queries.GetCommentById;

public class GetCommentByIdQueryValidator : AbstractValidator<GetCommentByIdQuery>
{
    public GetCommentByIdQueryValidator(IEntityRepository repository)
    {
        RuleFor(x => x.CommentId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Comment, CommentId>(id, token))
            .WithMessage("The comment with specified identifier does not exists.");
    }
}