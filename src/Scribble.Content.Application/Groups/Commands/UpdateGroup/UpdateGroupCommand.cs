using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Groups.Commands.UpdateGroup;

public class UpdateGroupCommand : ICommand
{
    public UpdateGroupCommand(GroupId groupId, string name, string shortName, string description)
    {
        GroupId = groupId;
        Name = name;
        ShortName = shortName;
        Description = description;
    }
    
    public GroupId GroupId { get; }
    public string Name { get; }
    public string ShortName { get; }
    public string Description { get; }
}