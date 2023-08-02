using AutoMapper;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Abstractions.Messaging;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Repositories;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Application.Posts.Queries.GetPostById;

internal class GetPostByIdQueryHandler : IQueryHandler<GetPostByIdQuery, PostResponse>
{
    private readonly IMapper _mapper;
    private readonly IEntityRepository _repository;
    private readonly ILinkService _linkService;

    public GetPostByIdQueryHandler(IMapper mapper, IEntityRepository repository, ILinkService linkService)
    {
        _mapper = mapper;
        _repository = repository;
        _linkService = linkService;
    }

    public async Task<Result<PostResponse>> Handle(GetPostByIdQuery query, CancellationToken token)
    {
        var post = await _repository.GetAsync<Post, PostId>(query.PostId, token)
            .ConfigureAwait(false);

        if (post is null)
            return Result.Failure<PostResponse>(Errors.Post.NotExistsById);

        var response = _mapper.Map<PostResponse>(post);
        
        AddLinksForPosts(response);

        return Result.Success(response);
    }
    
    private void AddLinksForPosts(PostResponse post)
    {
        post.Links.Add(
            _linkService.Generate(
                "GetPostById",
                new { postId = post.Id },
                "self", "GET"));
        
        post.Links.Add(
            _linkService.Generate(
                "UpdatePost",
                new { postId = post.Id },
                "update-post", "PUT"));
        
        post.Links.Add(
            _linkService.Generate(
                "RemovePost",
                new { postId = post.Id },
                "remove-post", "DELETE"));
    }
}