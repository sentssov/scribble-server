using AutoMapper;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Comments.Queries.GetCommentById;

internal class GetCommentByIdQueryHandler : IQueryHandler<GetCommentByIdQuery, CommentResponse>
{
    private readonly IMapper _mapper;
    private readonly IEntityRepository _repository;
    private readonly ILinkService _linkService;

    public GetCommentByIdQueryHandler(IMapper mapper, 
        IEntityRepository repository, ILinkService linkService)
    {
        _mapper = mapper;
        _repository = repository;
        _linkService = linkService;
    }

    public async Task<Result<CommentResponse>> Handle(GetCommentByIdQuery query, CancellationToken token)
    {
        var comment = await _repository.GetAsync<Comment, CommentId>(query.CommentId, token)
            .ConfigureAwait(false);

        if (comment is null)
            return Result.Failure<CommentResponse>(Errors.Comment.NotExistsById);

        var response = _mapper.Map<CommentResponse>(comment);
        
        AddLinksForComments(response);

        return Result.Success(response);
    }

    private void AddLinksForComments(CommentResponse response)
    {
        response.Links.Add(
            _linkService.Generate(
                "GetCommentById",
                new { commentId = response.Id },
                "self", "GET"));
        
        response.Links.Add(
            _linkService.Generate(
                "UpdateComment",
                new { commentId = response.Id },
                "update-comment", "PUT"));
        
        response.Links.Add(
            _linkService.Generate(
                "RemoveComment",
                new { commentId = response.Id },
                "remove-comment", "DELETE"));
    }
}