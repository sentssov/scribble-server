using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Models.ValueObjects;

public class GroupDescription : ValueObject
{
    private const int MaxLength = 400;

    private GroupDescription(string value) => 
        Value = value;

    public string Value { get; }
    
    public static explicit operator string(GroupDescription description) => description.Value;

    public static Result<GroupDescription> Create(string value)
    {
        if (value.Length > MaxLength)
        {
            return Result.Failure<GroupDescription>(new Error(
                $"{nameof(GroupDescription)}.TooLong", "The group description is too long."));
        }

        return new GroupDescription(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}