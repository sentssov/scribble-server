using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Comments.Commands.CreateComment;
using Scribble.Content.Application.Comments.Queries.GetComments;
using Scribble.Content.Application.Posts.Commands.CreateLike;
using Scribble.Content.Application.Posts.Commands.DeleteLike;
using Scribble.Content.Application.Posts.Commands.RemovePost;
using Scribble.Content.Application.Posts.Commands.UpdatePost;
using Scribble.Content.Application.Posts.Queries.GetPostById;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Shared;
using Scribble.Content.Presentation.Contracts;
using Scribble.Content.Presentation.Contracts.Comments.Requests;
using Scribble.Content.Presentation.Contracts.Posts.Requests;
using Scribble.Content.Presentation.Controllers.Base;
using Scribble.Content.Presentation.Extensions;
using Scribble.Identity.Authorization;

namespace Scribble.Content.Presentation.Controllers.V1.Entities;

/// <summary>
/// Represents the posts controller.
/// </summary>
[ApiController]
[Authorize(Policy = DefaultPolicies.All.Name)]
public class PostsController : ApiController
{
    /// <inheritdoc />
    public PostsController(ISender sender, IMapper mapper) 
        : base(sender, mapper)
    {
    }
    
    /// <summary>
    /// Gets the post with specified identifier, if it exists.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The post with specified identifier, if it exists.</returns>
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Posts.GetPostById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPostById([FromRoute] Guid postId,
        CancellationToken token)
    {
        return await Result.Create(
                new GetPostByIdQuery(new PostId(postId)))
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }

    /// <summary>
    /// Updates the post with specified identifier, if it exists.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <param name="request">The specified update post request.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpPut(ApiRoutes.Posts.UpdatePost)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdatePost([FromRoute] Guid postId, [FromBody] UpdatePostRequest request,
        CancellationToken token)
    {
        return await Result.Create(
                new UpdatePostCommand(new PostId(postId),
                    request.Title,
                    request.Content,
                    request.Description))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }

    /// <summary>
    /// Removes the post with specified identifier, if it exists.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpDelete(ApiRoutes.Posts.RemovePost)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemovePost([FromRoute] Guid postId,
        CancellationToken token)
    {
        return await Result.Create(
                new RemovePostCommand(new PostId(postId)))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }

    /// <summary>
    /// Gets post comments with specified identifier by specified filter.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <param name="pagination"></param>
    /// <param name="filter">The specified filter.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>Post comments with specified identifier by specified filter.</returns>
    [HttpGet(ApiRoutes.Posts.GetComments)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetComments([FromRoute] Guid postId, [FromQuery] GetQueryPagination pagination,
        [FromQuery] GetCommentsQueryFilter filter,
        CancellationToken token)
    {
        return await Result.Create(
                new GetCommentsQuery(new PostId(postId), pagination, filter))
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }
    
    /// <summary>
    /// Creates a new comment on the specified request for specified post.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <param name="request">The specified create comment request.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The identifier of the newly created comment.</returns>
    [HttpPost(ApiRoutes.Posts.CreateComment)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateComment([FromRoute] Guid postId, [FromBody] CreateCommentRequest request,
        CancellationToken token)
    {
        return await Result.Create(
                new CreateCommentCommand(new PostId(postId), new UserId(UserId),
                    request.Text))
            .Bind(command => Sender.Send(command, token))
            .Match(
                commentId => CreatedAtAction("GetCommentById", "Comments", new { commentId }, commentId),
                HandleFailure);
    }

    /// <summary>
    /// Creates a new like for the post with specified identifier.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpPost(ApiRoutes.Posts.CreateLike)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateLike([FromRoute] Guid postId,
        CancellationToken token)
    { 
        return await Result.Create(
                new CreateLikeCommand(new PostId(postId), new UserId(UserId)))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }
    
    /// <summary>
    /// Removes the like of post with specified identifier.
    /// </summary>
    /// <param name="postId">The post identifier.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpDelete(ApiRoutes.Posts.RemoveLike)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveLike([FromRoute] Guid postId,
        CancellationToken token)
    {
        return await Result.Create(
                new RemoveLikeCommand(new PostId(postId), new UserId(UserId)))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }
}