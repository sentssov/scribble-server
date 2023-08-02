using AutoMapper;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Tags.Queries.GetTagById;

internal class GetTagByIdQueryHandler : IQueryHandler<GetTagByIdQuery, TagResponse>
{
    private readonly IMapper _mapper;
    private readonly IEntityRepository _repository;
    private readonly ILinkService _linkService;

    public GetTagByIdQueryHandler(IMapper mapper, IEntityRepository repository, ILinkService linkService)
    {
        _mapper = mapper;
        _repository = repository;
        _linkService = linkService;
    }

    public async Task<Result<TagResponse>> Handle(GetTagByIdQuery query, CancellationToken token)
    {
        var tag = await _repository.GetAsync<Tag, TagId>(query.TagId, token)
            .ConfigureAwait(false);

        if (tag is null)
            return Result.Failure<TagResponse>(Errors.Tag.NotExistsById);

        var response = _mapper.Map<TagResponse>(tag);
        
        AddLinksForTags(response);

        return Result.Success(response);
    }
    
    private void AddLinksForTags(TagResponse tag)
    {
        tag.Links.Add(
            _linkService.Generate(
                "GetTagById",
                new { tagId = tag.Id },
                "self", "GET"));
        
        tag.Links.Add(
            _linkService.Generate(
                "UpdateTag",
                new { tagId = tag.Id },
                "update-tag", "PUT"));
        
        tag.Links.Add(
            _linkService.Generate(
                "RemoveTag",
                new { tagId = tag.Id },
                "remove-tag", "DELETE"));
    }
}