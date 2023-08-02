using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Groups.Commands.CreateSubscription;

public class CreateSubscriptionCommand : ICommand
{
    public CreateSubscriptionCommand(UserId userId, GroupId groupId)
    {
        UserId = userId;
        GroupId = groupId;
    }
    
    public GroupId GroupId { get; }
    public UserId UserId { get; }
}