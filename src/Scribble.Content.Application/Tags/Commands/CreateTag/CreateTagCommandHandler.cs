using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Tags.Commands.CreateTag;

internal class CreateTagCommandHandler : ICommandHandler<CreateTagCommand, TagId>
{
    private readonly IEntityRepository _repository;

    public CreateTagCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result<TagId>> Handle(CreateTagCommand command, CancellationToken token)
    {
        var isNameUnique = await _repository
            .UniqueByAsync<Tag, TagId>(x => x.Name.Value != command.Name, token)
            .ConfigureAwait(false);
        
        var tagName = TagName.Create(command.Name, isNameUnique).Value;

        var tagResult = Tag.Create(tagName, command.UserId);

        return await tagResult.Bind(t => _repository.SaveAsync<Tag, TagId>(t, token));
    }
}