using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Models.ValueObjects;

public class PostTitle : ValueObject
{
    private const int MaxLength = 75;

    private PostTitle(string value) => Value = value;

    public string Value { get; }

    public static explicit operator string(PostTitle title) => title.Value;

    public static Result<PostTitle> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<PostTitle>(new Error(
                $"{nameof(PostTitle)}.Empty", "The post title is empty."));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<PostTitle>(new Error(
                $"{nameof(PostTitle)}.TooLong", "The post title is too long."));
        }

        return new PostTitle(value);
    }
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}