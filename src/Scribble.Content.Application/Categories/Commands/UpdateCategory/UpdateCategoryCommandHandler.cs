using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Categories.Commands.UpdateCategory;

internal class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IEntityRepository _repository;

    public UpdateCategoryCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken token)
    {
        var isNameUnique = await _repository
            .UniqueByAsync<Category, CategoryId>(x => x.Name.Value != command.Name, token)
            .ConfigureAwait(false);
        
        var categoryName = CategoryName.Create(command.Name, isNameUnique).Value;

        var categoryResult = Result.Create(await _repository
            .GetAsync<Category, CategoryId>(command.CategoryId, token));

        return categoryResult.Bind(c => c.Update(categoryName));
    }
}