using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Tags.Commands.CreateTag;

public class CreateTagCommand : ICommand<TagId>
{
    public CreateTagCommand(UserId userId, string name)
    {
        UserId = userId;
        Name = name;
    }
    
    public UserId UserId { get; }
    public string Name { get; }
}