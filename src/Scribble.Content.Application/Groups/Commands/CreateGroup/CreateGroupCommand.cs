using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Groups.Commands.CreateGroup;

public class CreateGroupCommand : ICommand<GroupId>
{
    public CreateGroupCommand(UserId userId, string name, string shortName, string description)
    {
        UserId = userId;
        Name = name;
        ShortName = shortName;
        Description = description;
    }
    
    public string Name { get; }
    public string ShortName { get; }
    public string Description { get; }
    public UserId UserId { get; }
}