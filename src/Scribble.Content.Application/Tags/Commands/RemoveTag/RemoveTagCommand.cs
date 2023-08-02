using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Tags.Commands.RemoveTag;

public class RemoveTagCommand : ICommand
{
    public RemoveTagCommand(TagId tagId) => 
        TagId = tagId;
    
    public TagId TagId { get; }
}