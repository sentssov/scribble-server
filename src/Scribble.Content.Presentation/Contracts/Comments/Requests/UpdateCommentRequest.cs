namespace Scribble.Content.Presentation.Contracts.Comments.Requests;

/// <summary>
/// Represents an update comment text request.
/// </summary>
public class UpdateCommentRequest
{
    /// <summary>
    /// A comment text.
    /// </summary>
    public string Text { get; set; } = null!;
}