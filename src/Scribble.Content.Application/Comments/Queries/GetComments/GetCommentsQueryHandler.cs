using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Comments.Queries.GetComments;

internal class GetCommentsQueryHandler : IQueryHandler<GetCommentsQuery, IPagedCollection<CommentResponse>>
{
    private readonly IMapper _mapper;
    private readonly IEntityRepository _repository;
    private readonly ILinkService _linkService;

    public GetCommentsQueryHandler(IMapper mapper, IEntityRepository repository, ILinkService linkService)
    {
        _mapper = mapper;
        _repository = repository;
        _linkService = linkService;
    }

    public async Task<Result<IPagedCollection<CommentResponse>>> Handle(GetCommentsQuery request, CancellationToken token)
    {
        var query = _repository.Query<Comment, CommentId>();

        if (!string.IsNullOrWhiteSpace(request.Filter.SearchTerm))
        {
            query = query.Where(x =>
                ((string)x.Text).Contains(request.Filter.SearchTerm));
        }

        query = request.Filter.SortOrder?.ToLower() == SortOrderConstants.Descending 
            ? query.OrderByDescending(GetSortProperty(request)) 
            : query.OrderBy(GetSortProperty(request));

        var response = query
            .ProjectTo<CommentResponse>(_mapper.ConfigurationProvider);

        var categories = await PagedCollection<CommentResponse>
            .CreateAsync(response, 
                request.Pagination.PageIndex, request.Pagination.PageSize, token)
            .ConfigureAwait(false);
        
        AddLinksForComments(categories, request);

        return Result.Success(categories);
    }
    
    private static Expression<Func<Comment, object>> GetSortProperty(GetCommentsQuery request) =>
        request.Filter.SortColumn?.ToLower() switch
        {
            "text" => comment => comment.Text.Value,
            _ => comment => comment.Id
        };
    
    private void AddLinksForComments(IPagedCollection<CommentResponse> comments, GetCommentsQuery query)
    {
        comments.Links.Add(
            _linkService.Generate(
                "GetComments",
                new
                {
                    searchTerm = query.Filter.SearchTerm,
                    sortColumn = query.Filter.SortColumn,
                    sortOrder = query.Filter.SortOrder,
                    pageIndex = query.Pagination.PageIndex,
                    pageSize = query.Pagination.PageSize
                },
                "self", "GET"));
        
        if (comments.HasNextPage)
        {
            comments.Links.Add(
                _linkService.Generate(
                    "GetComments",
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

        if (comments.HasPreviousPage)
        {
            comments.Links.Add(
                _linkService.Generate(
                    "GetComments",
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