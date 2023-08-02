using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;

namespace Scribble.Content.Application.Categories.Commands.RemoveCategory;

public class RemoveCategoryCommand : ICommand
{
    public RemoveCategoryCommand(CategoryId categoryId) => 
        CategoryId = categoryId;
    
    public CategoryId CategoryId { get; }
}