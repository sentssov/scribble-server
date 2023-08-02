using FluentValidation;

namespace Scribble.Content.Application.Groups.Queries.GetGroupByShortName;

public class GetGroupByShortNameValidator : AbstractValidator<GetGroupByShortNameQuery>
{
    public GetGroupByShortNameValidator()
    {
        RuleFor(x => x.ShortName)
            .NotNull().NotEmpty().MaximumLength(45);
    }
}