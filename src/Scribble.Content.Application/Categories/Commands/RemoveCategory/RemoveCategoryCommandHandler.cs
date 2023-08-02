using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Categories.Commands.RemoveCategory;

internal class RemoveCategoryCommandHandler : ICommandHandler<RemoveCategoryCommand>
{
    private readonly IEntityRepository _repository;

    public RemoveCategoryCommandHandler(IEntityRepository repository) =>
        _repository = repository;

    public async Task<Result> Handle(RemoveCategoryCommand command, CancellationToken token)
    {
        await _repository.RemoveAsync<Category, CategoryId>(command.CategoryId, token)
            .ConfigureAwait(false);

        return Result.Success();
    }
}