using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Categories.Commands.ExcludeGroup;

public class ExcludeGroupCommand : ICommand
{
    public ExcludeGroupCommand(GroupId groupId, CategoryId categoryId)
    {
        GroupId = groupId;
        CategoryId = categoryId;
    }

    public GroupId GroupId { get; }
    public CategoryId CategoryId { get; }
}