using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Categories.Commands.CreateCategory;

internal class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryId>
{
    private readonly IEntityRepository _repository;

    public CreateCategoryCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result<CategoryId>> Handle(CreateCategoryCommand command, CancellationToken token)
    {
        var isNameUnique = await _repository
            .UniqueByAsync<Category, CategoryId>(x => x.Name.Value != command.Name, token)
            .ConfigureAwait(false);
        
        var categoryName = CategoryName.Create(command.Name, isNameUnique).Value;

        var category = Category.Create(categoryName, command.UserId).Value;

        return await _repository.SaveAsync<Category, CategoryId>(category, token)
            .ConfigureAwait(false);
    }
}