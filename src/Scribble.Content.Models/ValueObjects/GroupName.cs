using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Models.ValueObjects;

public class GroupName : ValueObject
{
    private const int MaxLength = 75;

    private GroupName(string value) => Value = value;

    public string Value { get; }
    
    public static explicit operator string(GroupName name) => name.Value;

    public static Result<GroupName> Create(string value, bool isUnique)
    {
        if (!isUnique)
        {
            return Result.Failure<GroupName>(new Error(
                $"{nameof(GroupName)}.NotUnique", "The group name is not unique."));
        }
        
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<GroupName>(new Error(
                $"{nameof(GroupName)}.Empty", "The group name is empty."));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<GroupName>(new Error(
                $"{nameof(GroupName)}.TooLong", "The group name is too long."));
        }

        return new GroupName(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}