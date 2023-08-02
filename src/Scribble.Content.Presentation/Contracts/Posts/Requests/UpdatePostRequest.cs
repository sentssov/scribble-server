namespace Scribble.Content.Presentation.Contracts.Posts.Requests;

public class UpdatePostRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
}