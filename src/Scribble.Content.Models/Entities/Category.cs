using System.Collections.ObjectModel;
using Scribble.Content.Models.Primitives;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;
using StronglyTypedIds;

namespace Scribble.Content.Models.Entities;

/// <summary>
/// 
/// </summary>
[StronglyTypedId(StronglyTypedIdBackingType.Guid, StronglyTypedIdConverter.NewtonsoftJson)]
public partial struct CategoryId {}

public class Category : Entity<CategoryId>
{
    private readonly Collection<Group> _groups = new();
    private Category(CategoryName name, UserId userId)
        : base(CategoryId.New())
    {
        Name = name;
        UserId = userId;
    }

    public CategoryName Name { get; private set; }
    public UserId UserId { get; }
    public virtual User User { get; } = null!;
    public virtual IReadOnlyCollection<Group> Groups => _groups.AsReadOnly();

    /// <summary>
    /// Creates a new category with specified name and user identifier.
    /// </summary>
    /// <param name="name">The specified category name.</param>
    /// <param name="userId">The user identifier.</param>
    /// <returns>Result.Success if the category was created successfully.</returns>
    public static Result<Category> Create(CategoryName name, UserId userId) => 
        new Category(name, userId);

    /// <summary>
    /// Update the category.
    /// </summary>
    /// <param name="name">The new category name.</param>
    /// <returns>Result.Success if the category was updated successfully.</returns>
    public Result Update(CategoryName name)
    {
        Name = name;

        return Result.Success();
    }

    /// <summary>
    /// Add the specified group into the category.
    /// </summary>
    /// <param name="group">The specified group.</param>
    /// <returns>Result.Success if the group has not been added to the category, otherwise CategoryErrors.GroupAlreadyIncluded</returns>
    public Result IncludeGroup(Group group)
    {
        var existingBlog = _groups.FirstOrDefault(x => x.Id == group.Id);

        if (existingBlog is not null)
        {
            return Result.Failure(Errors.Category.GroupAlreadyIncluded);
        }
        
        _groups.Add(group);

        return Result.Success();
    }

    /// <summary>
    /// Removes the specified group from the category.
    /// </summary>
    /// <param name="group">The specified group.</param>
    /// <returns>Result.Success if the group was found in the category, otherwise CategoryErrors.GroupHasNotBeenIncluded.</returns>
    public Result ExcludeGroup(Group group)
    {
        var existingGroup = _groups.FirstOrDefault(x => x.Id == group.Id);

        if (existingGroup is null)
        {
            return Result.Failure(Errors.Category.GroupHasNotBeenIncluded);
        }
        
        _groups.Remove(existingGroup);

        return Result.Success();
    }
}