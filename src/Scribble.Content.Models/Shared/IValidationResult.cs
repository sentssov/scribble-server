namespace Scribble.Content.Models.Shared;

public interface IValidationResult
{
    public static readonly Error ValidationError = new Error(
        "Validation Error",
        "Some validation errors occurred.");
    
    Error[] Errors { get; }
}