using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Tags.Queries.GetTagsCount;

public class GetTagsCountQueryHandler : IQueryHandler<GetTagsCountQuery, long>
{
    private readonly IEntityRepository _repository;

    public GetTagsCountQueryHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result<long>> Handle(GetTagsCountQuery query, CancellationToken token)
    {
        return Result.Create(await _repository
            .CountAsync<Tag, TagId>(t => t.UserId == query.UserId, token));
    }
}