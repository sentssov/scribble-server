using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Application.Tags.Commands.UpdateTag;

internal class UpdateTagCommandHandler : ICommandHandler<UpdateTagCommand>
{
    private readonly IEntityRepository _repository;

    public UpdateTagCommandHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result> Handle(UpdateTagCommand command, CancellationToken token)
    {
        var isNameUnique = await _repository
            .UniqueByAsync<Tag, TagId>(x => x.Name.Value != command.Name, token)
            .ConfigureAwait(false);
        
        var tagName = TagName.Create(command.Name, isNameUnique).Value;

        var tagResult = Result.Create(await _repository
            .GetAsync<Tag, TagId>(command.TagId, token));

        return tagResult.Bind(t => t.Update(tagName));
    }
}