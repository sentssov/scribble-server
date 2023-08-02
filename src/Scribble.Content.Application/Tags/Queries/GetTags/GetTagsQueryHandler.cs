using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Application.Groups.Queries;
using Scribble.Content.Application.Groups.Queries.GetGroups;
using Scribble.Content.Models;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Tags.Queries.GetTags;

internal class GetTagsQueryHandler : IQueryHandler<GetTagsQuery, IPagedCollection<TagResponse>>
{
    private readonly IMapper _mapper;
    private readonly IEntityRepository _repository;
    private readonly ILinkService _linkService;

    public GetTagsQueryHandler(IMapper mapper, IEntityRepository repository, ILinkService linkService)
    {
        _mapper = mapper;
        _repository = repository;
        _linkService = linkService;
    }

    public async Task<Result<IPagedCollection<TagResponse>>> Handle(GetTagsQuery request, CancellationToken token)
    {
        var query = _repository.Query<Tag, TagId>();

        if (!string.IsNullOrWhiteSpace(request.Filter.SearchTerm))
        {
            query = query.Where(x =>
                ((string)x.Name).Contains(request.Filter.SearchTerm));
        }

        query = request.Filter.SortOrder?.ToLower() == SortOrderConstants.Descending
            ? query.OrderByDescending(GetSortProperty(request))
            : query.OrderBy(GetSortProperty(request));

        var response = query
            .ProjectTo<TagResponse>(_mapper.ConfigurationProvider);

        var tags = await PagedCollection<TagResponse>
            .CreateAsync(response, request.Pagination.PageIndex, request.Pagination.PageSize, token)
            .ConfigureAwait(false);
        
        AddLinksForTags(tags, request);

        return Result.Success(tags);
    }
    
    private static Expression<Func<Tag, object>> GetSortProperty(GetTagsQuery request) =>
        request.Filter.SortColumn?.ToLower() switch
        {
            "name" => tag => tag.Name.Value,
            _ => tag => tag.Id
        };
    
    private void AddLinksForTags(IPagedCollection<TagResponse> tags, GetTagsQuery query)
    {
        tags.Links.Add(
            _linkService.Generate(
                "GetTags",
                new
                {
                    searchTerm = query.Filter.SearchTerm,
                    sortColumn = query.Filter.SortColumn,
                    sortOrder = query.Filter.SortOrder,
                    pageIndex = query.Pagination.PageIndex,
                    pageSize = query.Pagination.PageSize
                },
                "self", "GET"));
        
        if (tags.HasNextPage)
        {
            tags.Links.Add(
                _linkService.Generate(
                    "GetTags",
                    new
                    {
                        searchTerm = query.Filter.SearchTerm,
                        sortColumn = query.Filter.SortColumn,
                        sortOrder = query.Filter.SortOrder,
                        pageIndex = query.Pagination.PageIndex + 1,
                        pageSize = query.Pagination.PageSize
                    },
                    "next-page", "GET"));
        }

        if (tags.HasPreviousPage)
        {
            tags.Links.Add(
                _linkService.Generate(
                    "GetTags",
                    new
                    {
                        searchTerm = query.Filter.SearchTerm,
                        sortColumn = query.Filter.SortColumn,
                        sortOrder = query.Filter.SortOrder,
                        pageIndex = query.Pagination.PageIndex - 1,
                        pageSize = query.Pagination.PageSize
                    },
                    "previous-page", "GET"));
        }
    }
}