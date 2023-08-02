using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Application.Extensions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Groups.Queries.GetGroups;

internal class GetGroupsQueryHandler : IQueryHandler<GetGroupsQuery, IPagedCollection<GroupResponse>>
{
    private readonly IMapper _mapper;
    private readonly IEntityRepository _repository;
    private readonly ILinkService _linkService;

    public GetGroupsQueryHandler(IMapper mapper, IEntityRepository repository, ILinkService linkService)
    {
        _mapper = mapper;
        _repository = repository;
        _linkService = linkService;
    }

    public async Task<Result<IPagedCollection<GroupResponse>>> Handle(GetGroupsQuery request, CancellationToken token)
    {
        var filter = request.Filter;
        
        var query = _repository.Query<Group, GroupId>()
            .WhereIf(filter.UserId != null, 
                g => g.UserId.Value == filter.UserId)
            .WhereIf(filter.CategoryName != null, 
                g => g.Categories.Any(c => c.Name.Value == filter.CategoryName));

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(x =>
                ((string)x.Name).Contains(filter.SearchTerm) ||
                ((string)x.ShortName).Contains(filter.SearchTerm) ||
                ((string)x.Description).Contains(filter.SearchTerm));
        }

        query = filter.SortOrder?.ToLower() == SortOrderConstants.Descending 
            ? query.OrderByDescending(GetSortProperty(request)) 
            : query.OrderBy(GetSortProperty(request));

        var response = query
            .ProjectTo<GroupResponse>(_mapper.ConfigurationProvider);

        var groups = await PagedCollection<GroupResponse>
            .CreateAsync(response, request.Pagination.PageIndex, request.Pagination.PageSize, token)
            .ConfigureAwait(false);
        
        AddLinksForGroups(groups, request);

        return Result.Success(groups);
    }
    
    private static Expression<Func<Group, object>> GetSortProperty(GetGroupsQuery request) =>
        request.Filter.SortColumn?.ToLower() switch
        {
            "name" => group => group.Name.Value,
            "short-name" => group => group.ShortName.Value,
            "description" => group => group.Description.Value,
            _ => group => group.Id
        };
    
    private void AddLinksForGroups(IPagedCollection<GroupResponse> groups, GetGroupsQuery query)
    {
        groups.Links.Add(
            _linkService.Generate(
                "GetGroups",
                new
                {
                    searchTerm = query.Filter.SearchTerm,
                    sortColumn = query.Filter.SortColumn,
                    sortOrder = query.Filter.SortOrder,
                    pageIndex = query.Pagination.PageIndex,
                    pageSize = query.Pagination.PageSize
                },
                "self", "GET"));
        
        if (groups.HasNextPage)
        {
            groups.Links.Add(
                _linkService.Generate(
                    "GetGroups",
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

        if (groups.HasPreviousPage)
        {
            groups.Links.Add(
                _linkService.Generate(
                    "GetGroups",
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