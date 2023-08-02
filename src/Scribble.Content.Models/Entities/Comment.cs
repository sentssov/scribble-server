using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;
using StronglyTypedIds;

namespace Scribble.Content.Models.Entities;

[StronglyTypedId(StronglyTypedIdBackingType.Guid, StronglyTypedIdConverter.NewtonsoftJson)]
public partial struct CommentId { }

public class Comment : Entity<CommentId>, IAuditableEntity
{
    public Comment() { }
    private Comment(CommentText text, UserId user, PostId post)
        : base(CommentId.New())
    {
        Text = text;
        UserId = user;
        PostId = post;
    }

    public CommentText Text { get; private set; } = null!;
    public UserId UserId { get; }
    public virtual User User { get; } = null!;
    public PostId PostId { get; }
    public virtual Post Post { get; } = null!;

    public DateTime CreatedOnUtc { get; set; }
    
    public DateTime? ModifiedOnUtc { get; set; }

    /// <summary>
    /// Creates a new comment with specified comment text, user and post identifier.
    /// </summary>
    /// <param name="text">The specified comment text.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="postId">The post identifier.</param>
    /// <returns>Result.Success if the comment was created successfully.</returns>
    internal static Result<Comment> Create(CommentText text, UserId userId, PostId postId) => 
        new Comment(text, userId, postId);

    /// <summary>
    /// Updates the comment.
    /// </summary>
    /// <param name="text">The new comment text.</param>
    /// <returns>Result.Success if the comment was updated successfully.</returns>
    public Result Update(CommentText text)
    {
        Text = text;

        return Result.Success();
    }
}