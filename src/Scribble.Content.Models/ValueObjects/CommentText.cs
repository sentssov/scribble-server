using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Models.ValueObjects;

public class CommentText : ValueObject
{
    private CommentText(string value) => 
        Value = value;

    public string Value { get; }
    
    public static explicit operator string(CommentText text) => text.Value;

    public static Result<CommentText> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<CommentText>(new Error(
                $"{nameof(CommentText)}.Empty", "The comment text is empty."));
        }

        return new CommentText(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}