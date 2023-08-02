using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Models.ValueObjects;

public class CategoryName : ValueObject
{
    private const int MaxLength = 75;

    private CategoryName(string value) => Value = value;

    public string Value { get; }

    public static explicit operator string(CategoryName name) => name.Value;

    public static Result<CategoryName> Create(string value, bool isUnique)
    {
        if (!isUnique)
        {
            return Result.Failure<CategoryName>(new Error(
                $"{nameof(CategoryName)}.NotUnique", "The category name is not unique."));
        }
        
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<CategoryName>(new Error(
                $"{nameof(CategoryName)}.Empty", "The category name is empty."));
        }

        if (value.Length > MaxLength)
        {
            return Result.Failure<CategoryName>(new Error(
                $"{nameof(CategoryName)}.TooLong", "The category name is too long."));
        }

        return new CategoryName(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}