using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Groups.Commands.RemoveSubscription;

public class RemoveSubscriptionCommand : ICommand
{
    public RemoveSubscriptionCommand(UserId userId, GroupId groupId)
    {
        UserId = userId;
        GroupId = groupId;
    }

    public UserId UserId { get; }
    public GroupId GroupId { get; }
}