using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Groups.Queries.GetGroupsCount;

public class GetGroupsCountQueryHandler : IQueryHandler<GetGroupsCountQuery, long>
{
    private readonly IEntityRepository _repository;

    public GetGroupsCountQueryHandler(IEntityRepository repository) => 
        _repository = repository;

    public async Task<Result<long>> Handle(GetGroupsCountQuery query, CancellationToken token)
    {
        return Result.Create(await _repository
            .CountAsync<Group, GroupId>(token: token));
    }
}