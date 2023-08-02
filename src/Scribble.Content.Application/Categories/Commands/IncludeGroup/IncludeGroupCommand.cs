using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Categories.Commands.IncludeGroup;

public class IncludeGroupCommand : ICommand
{
    public IncludeGroupCommand(GroupId groupId, CategoryId categoryId)
    {
        GroupId = groupId;
        CategoryId = categoryId;
    }

    public GroupId GroupId { get; init; }
    public CategoryId CategoryId { get; init; }
}