using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Application.Comments.Commands.RemoveComment;
using Scribble.Content.Application.Comments.Commands.UpdateComment;
using Scribble.Content.Application.Comments.Queries.GetCommentById;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Shared;
using Scribble.Content.Presentation.Contracts;
using Scribble.Content.Presentation.Contracts.Comments.Requests;
using Scribble.Content.Presentation.Controllers.Base;
using Scribble.Content.Presentation.Extensions;
using Scribble.Identity.Authorization;

namespace Scribble.Content.Presentation.Controllers.V1.Entities;

/// <summary>
/// Represents the comments controller.
/// </summary>
[ApiController]
[Authorize(Policy = DefaultPolicies.All.Name)]
public class CommentsController : ApiController
{
    /// <inheritdoc />
    public CommentsController(ISender sender, IMapper mapper) 
        : base(sender, mapper)
    {
    }

    /// <summary>
    /// Gets the comment with specified identifier, if it exists.
    /// </summary>
    /// <param name="commentId">The comment identifier.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The comment with specified identifier, if it exists.</returns>
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Comments.GetCommentById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid commentId,
        CancellationToken token)
    {
        return await Result.Create(
                new GetCommentByIdQuery(new CommentId(commentId)))
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }
    
    /// <summary>
    /// Updates the comment with specified identifier, if it exists.
    /// </summary>
    /// <param name="commentId">The comment identifier.</param>
    /// <param name="request">The update comment request.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpPut(ApiRoutes.Comments.UpdateComment)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateComment([FromRoute] Guid commentId, [FromBody] UpdateCommentRequest request,
        CancellationToken token)
    {
        return await Result.Create(
                new UpdateCommentCommand(new CommentId(commentId),
                    request.Text))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }

    /// <summary>
    /// Removes the comment with specified identifier, if it exists.
    /// </summary>
    /// <param name="commentId">The comment identifier.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpDelete(ApiRoutes.Comments.RemoveComment)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveComment([FromRoute] Guid commentId, 
        CancellationToken token)
    {
        return await Result.Create(
                new RemoveCommentCommand(new CommentId(commentId)))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }
}