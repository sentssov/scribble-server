using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : ICommand<CategoryId>
{
    public CreateCategoryCommand(UserId userId, string name) 
    {
        UserId = userId;
        Name = name;
    }

    public UserId UserId { get; }
    public string Name { get; }
}