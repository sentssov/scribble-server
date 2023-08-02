using FluentValidation;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;

namespace Scribble.Content.Application.Comments.Queries.GetComments;

public class GetCommentsQueryValidator : AbstractValidator<GetCommentsQuery>
{
    public GetCommentsQueryValidator(IEntityRepository repository)
    {
        RuleFor(x => x.PostId)
            .MustAsync(async (id, token) => await repository.ExistsAsync<Post, PostId>(id, token))
            .WithMessage("The post with specified identifier does not exists.");
    }
}