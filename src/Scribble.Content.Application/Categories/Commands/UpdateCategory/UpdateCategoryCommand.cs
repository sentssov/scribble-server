using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : ICommand
{
    public UpdateCategoryCommand(CategoryId categoryId, string name)
    {
        CategoryId = categoryId;
        Name = name;
    }
    
    public CategoryId CategoryId { get; }
    public string Name { get; }
}