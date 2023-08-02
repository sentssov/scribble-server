using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Models.ValueObjects;

public class PostDescription : ValueObject
{
    private const int MaxLength = 400;

    private PostDescription(string value) => Value = value;

    public string Value { get; }

    public static explicit operator string(PostDescription description) => description.Value;

    public static Result<PostDescription> Create(string value)
    {
        if (value.Length > MaxLength)
        {
            return Result.Failure<PostDescription>(new Error(
                $"{nameof(PostDescription)}.TooLong", "The post description is too long."));
        }

        return new PostDescription(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}