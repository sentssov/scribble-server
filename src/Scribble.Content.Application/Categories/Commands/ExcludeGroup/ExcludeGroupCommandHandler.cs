using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Categories.Commands.ExcludeGroup;

internal class ExcludeGroupCommandHandler : ICommandHandler<ExcludeGroupCommand>
{
    private readonly IEntityRepository _repository;

    public ExcludeGroupCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result> Handle(ExcludeGroupCommand command, CancellationToken token)
    {
        var categoryResult = Result.Create(await _repository
            .GetAsync<Category, CategoryId>(command.CategoryId, token));
        var groupResult = Result.Create(await _repository
            .GetAsync<Group, GroupId>(command.GroupId, token));

        return Result.Combine(categoryResult, groupResult)
            .Bind(() => categoryResult.Value.ExcludeGroup(groupResult.Value));
    }
}