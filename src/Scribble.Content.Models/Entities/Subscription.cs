using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;
using StronglyTypedIds;

namespace Scribble.Content.Models.Entities;

[StronglyTypedId(StronglyTypedIdBackingType.Guid, StronglyTypedIdConverter.NewtonsoftJson)]
public partial struct SubscriptionId { }

public class Subscription : Entity<SubscriptionId>, IAuditableEntity
{
    public Subscription() { }
    private Subscription(UserId userId, GroupId groupId) 
        : base(SubscriptionId.New())
    {
        UserId = userId;
        GroupId = groupId;
    }
    
    public UserId UserId { get; }
    public virtual User User { get; } = null!;
    public GroupId GroupId { get; }
    
    public virtual Group Group { get; } = null!;
    
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    internal static Result<Subscription> Create(UserId userId, GroupId groupId) => 
        new Subscription(userId, groupId);
}