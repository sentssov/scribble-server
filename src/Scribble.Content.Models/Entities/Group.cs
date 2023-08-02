using System.Collections.ObjectModel;
using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;
using StronglyTypedIds;

namespace Scribble.Content.Models.Entities;

[StronglyTypedId(StronglyTypedIdBackingType.Guid, StronglyTypedIdConverter.NewtonsoftJson)]
public partial struct GroupId { }

public class Group : AggregateRoot<GroupId>, IAuditableEntity
{
    private readonly Collection<Post> _posts = new();
    private readonly Collection<Category> _categories = new();
    private readonly Collection<Subscription> _subscriptions = new();

    private Group(GroupName name, GroupShortName shortName, UserId userId, GroupDescription description)
        : base(GroupId.New())
    {
        Name = name;
        ShortName = shortName;
        UserId = userId;
        Description = description;
    }

    public GroupName Name { get; private set; }
    public GroupShortName ShortName { get; private set; }
    public GroupDescription Description { get; private set; }
    public UserId UserId { get; private set; }
    public virtual User User { get; } = null!;
    
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }

    public virtual IReadOnlyCollection<Post> Posts => _posts.AsReadOnly();
    public virtual IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();
    public virtual IReadOnlyCollection<Subscription> Subscriptions => _subscriptions.AsReadOnly();

    /// <summary>
    /// Creates a new group with specified group name, short name, description and user identifier.
    /// </summary>
    /// <param name="name">The specified group name.</param>
    /// <param name="shortName">The specified group short name.</param>
    /// <param name="description">The specified group description.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns>Result.Success if the group was created successfully.</returns>
    public static Result<Group> Create(GroupName name, GroupShortName shortName, GroupDescription description, UserId userId) => 
        new Group(name, shortName, userId, description);

    /// <summary>
    /// Updates the group.
    /// </summary>
    /// <param name="name">The new group name.</param>
    /// <param name="shortName">The new group short name.</param>
    /// <param name="description">The new group description.</param>
    /// <returns>Result.Success if the group was updated successfully.</returns>
    public Result Update(GroupName name, GroupShortName shortName, GroupDescription description)
    {
        Name = name;
        ShortName = shortName;
        Description = description;

        return Result.Success();
    }

    /// <summary>
    /// Creates a new post with specified post title, content and description.
    /// </summary>
    /// <param name="title">The specified post title.</param>
    /// <param name="content">The specified post content.</param>
    /// <param name="description">The specified post description.</param>
    /// <returns>Result.Success with created post if the post was created successfully,
    /// otherwise Result.Failure with validation errors.</returns>
    public Result<Post> CreatePost(PostTitle title, PostContent content, PostDescription description)
    {
        var postResult = Post.Create(title, content, Id, description);

        if (postResult.IsFailure)
            return postResult;
        
        _posts.Add(postResult.Value);

        return postResult;
    }

    /// <summary>
    /// Removes the post with specified identifier from the group.
    /// </summary>
    /// <param name="postId">The specified post identifier.</param>
    /// <returns>Result.Success if the post with specified identifier was found, otherwise Result.Failure.</returns>
    public Result RemovePost(PostId postId)
    {
        var post = _posts.FirstOrDefault(x => x.Id == postId);

        if (post is null)
            return Result.Failure(Errors.Post.NotExistsById);

        _posts.Remove(post);

        return Result.Success();
    }

    /// <summary>
    /// Creates a new subscription on the group with specified user identifier.
    /// </summary>
    /// <param name="userId">The specified user identifier.</param>
    /// <returns>Result.Success if the user with specified identifier has not been subscribed,
    /// otherwise Result.Failure with validation errors.</returns>
    public Result<Subscription> CreateSubscription(UserId userId)
    {
        var subscription = _subscriptions.FirstOrDefault(
            x => x.GroupId == Id && x.UserId == userId);

        if (subscription is not null)
            return Result.Failure<Subscription>(Errors.Group.UserAlreadySubscribed);

        var subscriptionResult = Subscription.Create(userId, Id);
        
        _subscriptions.Add(subscriptionResult.Value);

        return subscriptionResult;
    }

    /// <summary>
    /// Removes the user subscription with specified user identifier from the group.
    /// </summary>
    /// <param name="userId">The specified user identifier.</param>
    /// <returns>Result.Success if the user with specified identifier has been subscribed on the group,
    /// otherwise Result.Failure.</returns>
    public Result RemoveSubscription(UserId userId)
    {
        var subscription = _subscriptions.FirstOrDefault(
            x => x.GroupId == Id && x.UserId == userId);

        if (subscription is null)
            return Result.Failure(Errors.Group.UserHasNotBeenSubscribed);

        _subscriptions.Remove(subscription);

        return Result.Success();
    }
}
