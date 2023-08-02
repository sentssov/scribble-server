using AutoMapper;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Groups.Queries.GetGroupByShortName;

public class GetGroupByShortNameQueryHandler : IQueryHandler<GetGroupByShortNameQuery, GroupResponse>
{
    private readonly IMapper _mapper;
    private readonly IEntityRepository _repository;
    private readonly ILinkService _linkService;

    public GetGroupByShortNameQueryHandler(IMapper mapper, IEntityRepository repository, ILinkService linkService)
    {
        _mapper = mapper;
        _repository = repository;
        _linkService = linkService;
    }

    public async Task<Result<GroupResponse>> Handle(GetGroupByShortNameQuery query, CancellationToken token)
    {
        var group = await _repository.GetAsync<Group, GroupId>(
                x => x.ShortName.Value == query.ShortName, token)
            .ConfigureAwait(false);

        if (group is null)
            return Result.Failure<GroupResponse>(Errors.Group.NotExistsByShortName);

        var response = _mapper.Map<GroupResponse>(group);
        
        AddLinksForGroups(response);

        return Result.Success(response);
    }
    
    private void AddLinksForGroups(GroupResponse group)
    {
        group.Links.Add(
            _linkService.Generate(
                "GetGroupByShortName",
                new { shortName = group.ShortName },
                "self", "GET"));
        
        // group.Links.Add(
        //     _linkService.Generate(
        //         "UpdateGroup",
        //         new { groupId = group.Id },
        //         "update-group", "PUT"));
        //
        // group.Links.Add(
        //     _linkService.Generate(
        //         "RemoveGroup",
        //         new { groupId = group.Id },
        //         "remove-group", "DELETE"));
    }
}