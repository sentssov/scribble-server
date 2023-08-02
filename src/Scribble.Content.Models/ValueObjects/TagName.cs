using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Models.ValueObjects;

public class TagName : ValueObject
{
    private const int MaxLength = 45;

    private TagName(string value) => Value = value;

    public string Value { get; }

    public static explicit operator string(TagName name) => name.Value;

    public static Result<TagName> Create(string value, bool isUnique)
    {
        if (!isUnique)
        {
            return Result.Failure<TagName>(new Error(
                $"{nameof(TagName)}.NotUnique", "The tag name is not unique."));
        }
        
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<TagName>(new Error(
                $"{nameof(TagName)}.Empty", "The tag name is empty."));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<TagName>(new Error(
                $"{nameof(TagName)}.TooLong", "The tag name is too long."));
        }

        return new TagName(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;

    }
}