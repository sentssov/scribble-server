using System.Collections.ObjectModel;
using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;
using StronglyTypedIds;

namespace Scribble.Content.Models.Entities;

[StronglyTypedId(StronglyTypedIdBackingType.Guid, StronglyTypedIdConverter.NewtonsoftJson)]
public partial struct TagId { }

public class Tag : Entity<TagId>, IAuditableEntity
{
    private readonly Collection<Post> _posts = new();

    public Tag() { }
    private Tag(TagName name, UserId userId) 
        : base(TagId.New())
    {
        Name = name;
        UserId = userId;
    }
    
    public TagName Name { get; private set; } = null!;
    public UserId UserId { get; }

    public virtual User User { get; } = null!;
    
    public DateTime CreatedOnUtc { get; set; }
    
    public DateTime? ModifiedOnUtc { get; set; }

    public virtual ReadOnlyCollection<Post> Posts => _posts.AsReadOnly();

    public static Result<Tag> Create(TagName name, UserId userId) => 
        new Tag(name, userId);

    public Result Update(TagName name)
    {
        Name = name;
        return Result.Success();
    }
}
