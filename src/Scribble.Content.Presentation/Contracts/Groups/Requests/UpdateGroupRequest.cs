namespace Scribble.Content.Presentation.Contracts.Groups.Requests;

public class UpdateGroupRequest
{
    public string Name { get; set; } = null!;
    public string ShortName { get; set; } = null!;
    public string Description { get; set; } = null!;
}