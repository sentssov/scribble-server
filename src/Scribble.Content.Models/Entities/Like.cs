using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;
using StronglyTypedIds;

namespace Scribble.Content.Models.Entities;

[StronglyTypedId(StronglyTypedIdBackingType.Guid, StronglyTypedIdConverter.NewtonsoftJson)]
public partial struct LikeId { }

public class Like : Entity<LikeId>, IAuditableEntity
{
    public Like() { }
    public Like(PostId postId, UserId userId)
        : base(LikeId.New())
    {
        PostId = postId;
        UserId = userId;
    }

    public PostId PostId { get; }
    public virtual Post Post { get; } = null!;
    public UserId UserId { get; }
    public virtual User User { get; } = null!;
    
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    internal static Result<Like> Create(PostId postId, UserId userId) =>
        new Like(postId, userId);
}