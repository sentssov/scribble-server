using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Groups.Commands.RemoveGroup;

public class RemoveGroupCommand : ICommand
{
    public RemoveGroupCommand(GroupId groupId) => 
        GroupId = groupId;
    
    public GroupId GroupId { get; set; }
}