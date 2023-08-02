using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Categories.Queries.GetCategories;

internal class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, IPagedCollection<CategoryResponse>>
{
    private readonly IMapper _mapper;
    private readonly IEntityRepository _repository;
    private readonly ILinkService _linkService;

    public GetCategoriesQueryHandler(IMapper mapper, IEntityRepository repository, ILinkService linkService)
    {
        _mapper = mapper;
        _repository = repository;
        _linkService = linkService;
    }

    public async Task<Result<IPagedCollection<CategoryResponse>>> Handle(GetCategoriesQuery request, CancellationToken token)
    {
        var query = _repository.Query<Category, CategoryId>();

        if (!string.IsNullOrWhiteSpace(request.Filter.SearchTerm))
        {
            query = query.Where(x =>
                ((string)x.Name).Contains(request.Filter.SearchTerm));
        }

        query = request.Filter.SortOrder?.ToLower() == SortOrderConstants.Descending 
            ? query.OrderByDescending(GetSortProperty(request)) 
            : query.OrderBy(GetSortProperty(request));

        var response = query
            .ProjectTo<CategoryResponse>(_mapper.ConfigurationProvider);

        var categories = await PagedCollection<CategoryResponse>
            .CreateAsync(response, 
                request.Pagination.PageIndex, 
                request.Pagination.PageSize, token)
            .ConfigureAwait(false);
        
        AddLinksForPagedCategories(categories, request);

        return Result.Success(categories);
    }

    private static Expression<Func<Category, object>> GetSortProperty(GetCategoriesQuery request) =>
        request.Filter.SortColumn?.ToLower() switch
        {
            "name" => category => category.Name,
            _ => category => category.Id
        };

    private void AddLinksForPagedCategories(IPagedCollection<CategoryResponse> categories, GetCategoriesQuery query)
    {
        categories.Links.Add(
            _linkService.Generate(
                "GetCategories",
                new
                {
                    searchTerm = query.Filter.SearchTerm,
                    sortColumn = query.Filter.SortColumn,
                    sortOrder = query.Filter.SortOrder,
                    pageIndex = query.Pagination.PageIndex,
                    pageSize = query.Pagination.PageSize
                },
                "self", "GET"));
        
        if (categories.HasNextPage)
        {
            categories.Links.Add(
                _linkService.Generate(
                    "GetCategories",
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

        if (categories.HasPreviousPage)
        {
            categories.Links.Add(
                _linkService.Generate(
                    "GetCategories",
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