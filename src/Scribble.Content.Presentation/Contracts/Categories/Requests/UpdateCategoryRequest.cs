namespace Scribble.Content.Presentation.Contracts.Categories.Requests;

/// <summary>
/// Represents a category request.
/// </summary>
public class UpdateCategoryRequest
{
    /// <summary>
    /// A name.
    /// </summary>
    public string Name { get; set; } = null!;
}