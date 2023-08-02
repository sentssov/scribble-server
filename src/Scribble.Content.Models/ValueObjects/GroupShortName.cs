using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Models.ValueObjects;

public class GroupShortName : ValueObject
{
    private const int MaxLength = 45;

    private GroupShortName(string value) => 
        Value = value;

    public string Value { get; }
    
    public static explicit operator string(GroupShortName shortName) => shortName.Value;

    public static Result<GroupShortName> Create(string value, bool isUnique)
    {
        if (!isUnique)
        {
            return Result.Failure<GroupShortName>(new Error(
                $"{nameof(GroupShortName)}.NotUnique", "The group short name is not unique."));
        }
        
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<GroupShortName>(new Error(
                $"{nameof(GroupShortName)}.Empty", "The group short name is empty."));
        }
        
        if (value.Length > MaxLength)
        {
            return Result.Failure<GroupShortName>(new Error(
                $"{nameof(GroupShortName)}.TooLong", "The group short name is too long."));
        }

        return new GroupShortName(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}