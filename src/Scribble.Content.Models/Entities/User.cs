using System.Collections.ObjectModel;
using Scribble.Content.Models.Primitives;
using StronglyTypedIds;

namespace Scribble.Content.Models.Entities;

[StronglyTypedId(StronglyTypedIdBackingType.Guid, StronglyTypedIdConverter.NewtonsoftJson)]
public partial struct UserId { }
public class User : Entity<UserId>
{
    private readonly Collection<Category> _categories = new();
    private readonly Collection<Comment> _comments = new();
    private readonly Collection<Group> _groups = new();
    private readonly Collection<Like> _likes = new();
    private readonly Collection<Tag> _tags = new();

    public User(UserId id) 
        : base(id)
    {
    }

    public virtual IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();
    public virtual IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
    public virtual IReadOnlyCollection<Group> Groups => _groups.AsReadOnly();
    public virtual IReadOnlyCollection<Like> Likes => _likes.AsReadOnly();
    public virtual IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();
}