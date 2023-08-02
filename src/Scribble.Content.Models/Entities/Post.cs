using System.Collections.ObjectModel;
using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;
using StronglyTypedIds;

namespace Scribble.Content.Models.Entities;

[StronglyTypedId(StronglyTypedIdBackingType.Guid, StronglyTypedIdConverter.NewtonsoftJson)]
public partial struct PostId {}

public class Post : AggregateRoot<PostId>, IAuditableEntity
{
    private readonly Collection<Tag> _tags = new();
    private readonly Collection<Like> _likes = new();
    private readonly Collection<Comment> _comments = new();

    private Post(PostTitle title, PostContent content, GroupId groupId, PostDescription description) 
        : base(PostId.New())
    {
        Title = title;
        Content = content;
        GroupId = groupId;
        Description = description;
    }
    
    public PostTitle Title { get; private set; }
    public PostContent Content { get; private set; }
    public PostDescription Description { get; private set; }
    
    public GroupId GroupId { get; }
    public virtual Group Group { get; } = null!;
    
    public DateTime CreatedOnUtc { get; set; }
    
    public DateTime? ModifiedOnUtc { get; set; }

    public virtual IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
    public virtual IReadOnlyCollection<Like> Likes => _likes.AsReadOnly();
    public virtual IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

    internal static Result<Post> Create(PostTitle title, PostContent content, GroupId groupId, PostDescription description) => 
        new Post(title, content, groupId, description);

    public Result Update(PostTitle title, PostContent content, PostDescription description)
    {
        Title = title;
        Content = content;
        Description = description;

        return Result.Success();
    }

    public Result AddTag(Tag tag)
    {
        var existingTag = _tags.FirstOrDefault(x => x.Id == tag.Id);

        if (existingTag is not null)
        {
            return Result.Failure<Tag>(new Error($"{nameof(Post)}.{nameof(AddTag)}",
            $"The tag with specified identifier '{tag.Id}' has already been added to the post with identifier '{Id}'."));
        }
        
        _tags.Add(tag);

        return Result.Success();
    }

    public Result RemoveTag(TagId tagId)
    {
        var existingTag = _tags.FirstOrDefault(x => x.Id == tagId);

        if (existingTag is null)
        {
            return Result.Failure(new Error($"{nameof(Post)}.{nameof(RemoveTag)}",
                $"The tag with specified identifier '{tagId} has not been added to the post with identifier {Id}.'"));
        }
        
        _tags.Remove(existingTag);

        return Result.Success();
    }

    public Result<Like> AddLike(UserId userId)
    {
        var existingLike = _likes.FirstOrDefault(x => x.UserId == userId);

        if (existingLike is not null)
        {
            return Result.Failure<Like>(new Error($"{nameof(Post)}.{nameof(AddLike)}",
                $"The user with specified identifier '{userId}' has already liked the post with the identifier '{Id}'."));
        }

        var likeResult = Like.Create(Id, userId);
        
        _likes.Add(likeResult.Value);

        return likeResult;
    }

    public Result RemoveLike(UserId userId)
    {
        var existingLike = _likes.FirstOrDefault(x => x.UserId == userId);

        if (existingLike is null)
        {
            return Result.Failure(new Error($"{nameof(Post)}.{nameof(RemoveLike)}",
                $"The user with specified identifier '{userId}' has not yet liked the post with the identifier '{Id}'."));
        }

        _likes.Remove(existingLike);

        return Result.Success();
    }

    public Comment? GetComment(CommentId commentId)
    {
        return _comments.FirstOrDefault(x => x.Id == commentId);
    }

    public Result<Comment> CreateComment(CommentText text, UserId userId)
    {
        var commentResult = Comment.Create(text, userId, Id);

        if (commentResult.IsFailure)
            return commentResult;
        
        _comments.Add(commentResult.Value);

        return commentResult;
    }

    public Result RemoveComment(CommentId commentId)
    {
        var comment = _comments.FirstOrDefault(x => x.Id == commentId);

        if (comment is null)
        {
            return Result.Failure(new Error($"{nameof(Post)}.{nameof(RemoveComment)}",
                $"The comment with specified identifier '{commentId}' does not exists."));
        }

        _comments.Remove(comment);

        return Result.Success();
    }
}