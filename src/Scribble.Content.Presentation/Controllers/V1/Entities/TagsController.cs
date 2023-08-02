using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Tags.Commands.CreateTag;
using Scribble.Content.Application.Tags.Commands.RemoveTag;
using Scribble.Content.Application.Tags.Commands.UpdateTag;
using Scribble.Content.Application.Tags.Queries.GetTagById;
using Scribble.Content.Application.Tags.Queries.GetTags;
using Scribble.Content.Application.Tags.Queries.GetTagsCount;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Shared;
using Scribble.Content.Presentation.Contracts;
using Scribble.Content.Presentation.Contracts.Tags.Requests;
using Scribble.Content.Presentation.Controllers.Base;
using Scribble.Content.Presentation.Extensions;
using Scribble.Identity.Authorization;

namespace Scribble.Content.Presentation.Controllers.V1.Entities;

/// <summary>
/// Represents the tags controller.
/// </summary>
[ApiController]
[Authorize(Policy = DefaultPolicies.All.Name)]
public class TagsController : ApiController
{
    /// <inheritdoc />
    public TagsController(ISender sender, IMapper mapper) 
        : base(sender, mapper)
    {
    }

    /// <summary>
    /// Gets the tag with specified identifier, if it exists.
    /// </summary>
    /// <param name="tagId">The tag identifier.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The tag with specified identifier, if it exists.</returns>
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Tags.GetTagById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTagById([FromRoute] Guid tagId,
        CancellationToken token)
    {
        return await Result.Create(
                new GetTagByIdQuery(new TagId(tagId)))
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }

    /// <summary>
    /// Gets all created tags with specified user identifier.
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="filter">The request filter.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>All created tags with specified user identifier.</returns>
    [HttpGet(ApiRoutes.Tags.GetTags)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTags([FromQuery] GetQueryPagination pagination, 
        [FromQuery] GetTagsQueryFilter filter,
        CancellationToken token)
    {
        return await Result.Create(
                new GetTagsQuery(new UserId(UserId), pagination, filter))
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }

    /// <summary>
    /// Gets total user count with specified user identifier.
    /// </summary>
    /// <param name="token">The cancellation token.</param>
    /// <returns>Total user tags count.</returns>
    public async Task<IActionResult> GetTagsCount(
        CancellationToken token)
    {
        return await Result.Create(
                new GetTagsCountQuery(new UserId(UserId)))
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }

    /// <summary>
    /// Creates a new tag based on the specified request.
    /// </summary>
    /// <param name="request">The create tag request.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The identifier of the newly created tag.</returns>
    [HttpPost(ApiRoutes.Tags.CreateTag)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagRequest request,
        CancellationToken token)
    {
        return await Result.Create(
                new CreateTagCommand(new UserId(UserId), 
                    request.Name))
            .Bind(command => Sender.Send(command, token))
            .Match(
                tagId => CreatedAtAction(nameof(GetTagById), new { tagId }, tagId),
                HandleFailure);
    }
 
    /// <summary>
    /// Updates the tag name with specified identifier.
    /// </summary>
    /// <param name="tagId">The tag identifier..</param>
    /// <param name="request">The update tag name request.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpPut(ApiRoutes.Tags.UpdateTag)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateTag([FromRoute] Guid tagId, [FromBody] UpdateTagRequest request,
        CancellationToken token)
    {
        return await Result.Create(
                new UpdateTagCommand(new TagId(tagId), 
                    request.Name))
            .Bind(command => 
                Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }

    /// <summary>
    /// Removes the tag with specified identifier, if it exists.
    /// </summary>
    /// <param name="tagId">The tag identifier.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpDelete(ApiRoutes.Tags.RemoveTag)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveTag([FromRoute] Guid tagId,
        CancellationToken token)
    {
        return await Result.Create(
                new RemoveTagCommand(new TagId(tagId)))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }
}