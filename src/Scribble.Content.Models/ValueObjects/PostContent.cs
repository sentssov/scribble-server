using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Models.ValueObjects;

public class PostContent : ValueObject
{
    private PostContent(string value) => Value = value;

    public string Value { get; }
    
    public static explicit operator string(PostContent content) => content.Value;

    public static Result<PostContent> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<PostContent>(new Error(
                $"{nameof(PostContent)}.Empty", "The post content is empty."));
        }

        return new PostContent(value);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}