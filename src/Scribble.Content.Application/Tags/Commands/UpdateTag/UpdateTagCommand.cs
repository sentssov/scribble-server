using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Tags.Commands.UpdateTag;

public class UpdateTagCommand : ICommand
{
    public UpdateTagCommand(TagId tagId, string name)
    {
        TagId = tagId;
        Name = name;
    }

    public TagId TagId { get; }
    public string Name { get; }
}