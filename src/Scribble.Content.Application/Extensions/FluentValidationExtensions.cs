using FluentValidation;
using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeValueObject<T, TValueObject>(
        this IRuleBuilder<T, string> ruleBuilder, Func<string, Result<TValueObject>> factoryMethod)
        where TValueObject : ValueObject
    {
        return (IRuleBuilderOptions<T, string>)ruleBuilder
            .Custom((value, context) =>
            {
                var result = factoryMethod(value);

                foreach (var error in result.Errors)
                {
                    context.AddFailure(error.Message);
                }
            });
    }
    
    public static IRuleBuilderOptions<T, string> MustBeValueObject<T, TValueObject>(
        this IRuleBuilder<T, string> ruleBuilder, Func<string, Task<Result<TValueObject>>> factoryMethod)
        where TValueObject : ValueObject
    {
        return (IRuleBuilderOptions<T, string>)ruleBuilder
            .Custom(async (value, context) =>
            {
                var result = await factoryMethod(value);

                foreach (var error in result.Errors)
                {
                    context.AddFailure(error.Message);
                }
            });
    }
}