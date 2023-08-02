using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Application.Extensions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Posts.Queries.GetPosts;

internal class GetPostsQueryHandler : IQueryHandler<GetPostsQuery, IPagedCollection<PostResponse>>
{
    private readonly IMapper _mapper;
    private readonly IEntityRepository _repository;
    private readonly ILinkService _linkService;

    public GetPostsQueryHandler(IMapper mapper, IEntityRepository repository, ILinkService linkService)
    {
        _mapper = mapper;
        _repository = repository;
        _linkService = linkService;
    }

    public async Task<Result<IPagedCollection<PostResponse>>> Handle(GetPostsQuery request, CancellationToken token)
    {
        var filter = request.Filter;
        
        var query = _repository.Query<Post, PostId>()
            .WhereIf(filter.GroupId != null, x => x.GroupId.Value == filter.GroupId);

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(x =>
                ((string)x.Title).Contains(filter.SearchTerm));
        }

        query = filter.SortOrder?.ToLower() == SortOrderConstants.Descending 
            ? query.OrderByDescending(GetSortProperty(request)) 
            : query.OrderBy(GetSortProperty(request));

        var response = query
            .ProjectTo<PostResponse>(_mapper.ConfigurationProvider);

        var posts = await PagedCollection<PostResponse>
            .CreateAsync(response, request.Pagination.PageIndex, request.Pagination.PageSize, token)
            .ConfigureAwait(false);
        
        AddLinksForPosts(posts, request);

        return Result.Success(posts);
    }
    
    private static Expression<Func<Post, object>> GetSortProperty(GetPostsQuery request) =>
        request.Filter.SortColumn?.ToLower() switch
        {
            "title" => post => post.Title.Value,
            _ => post => post.Id
        };
    
    private void AddLinksForPosts(IPagedCollection<PostResponse> posts, GetPostsQuery query)
    {
        posts.Links.Add(
            _linkService.Generate(
                "GetPosts",
                new
                {
                    searchTerm = query.Filter.SearchTerm,
                    sortColumn = query.Filter.SortColumn,
                    sortOrder = query.Filter.SortOrder,
                    pageIndex = query.Pagination.PageIndex,
                    pageSize = query.Pagination.PageSize
                },
                "self", "GET"));
        
        if (posts.HasNextPage)
        {
            posts.Links.Add(
                _linkService.Generate(
                    "GetPosts",
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

        if (posts.HasPreviousPage)
        {
            posts.Links.Add(
                _linkService.Generate(
                    "GetPosts",
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