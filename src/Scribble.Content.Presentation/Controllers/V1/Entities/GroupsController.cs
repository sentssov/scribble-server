using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Groups.Commands.CreateGroup;
using Scribble.Content.Application.Groups.Commands.CreateSubscription;
using Scribble.Content.Application.Groups.Commands.RemoveGroup;
using Scribble.Content.Application.Groups.Commands.RemoveSubscription;
using Scribble.Content.Application.Groups.Commands.UpdateGroup;
using Scribble.Content.Application.Groups.Queries.GetGroupById;
using Scribble.Content.Application.Groups.Queries.GetGroupByShortName;
using Scribble.Content.Application.Groups.Queries.GetGroups;
using Scribble.Content.Application.Groups.Queries.GetGroupsCount;
using Scribble.Content.Application.Posts.Commands.CreatePost;
using Scribble.Content.Application.Posts.Queries.GetPosts;
using Scribble.Content.Presentation.Controllers.Base;
using Scribble.Content.Presentation.Extensions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Shared;
using Scribble.Content.Presentation.Contracts;
using Scribble.Content.Presentation.Contracts.Groups.Requests;
using Scribble.Content.Presentation.Contracts.Posts.Requests;
using Scribble.Identity.Authorization;

namespace Scribble.Content.Presentation.Controllers.V1.Entities;

/// <summary>
/// Represents the groups controller.
/// </summary>
[ApiController]
[Authorize(Policy = DefaultPolicies.All.Name)]
public class GroupsController : ApiController
{
    /// <inheritdoc />
    public GroupsController(ISender sender, IMapper mapper)
        : base(sender, mapper)
    {
    }
    
    /// <summary>
    /// Gets the group with specified identifier, if it exists.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The group with specified identifier, if it exists.</returns>
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Groups.GetGroupById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroupById([FromRoute] Guid groupId, 
        CancellationToken token)
    {
        return await Result.Create(
                new GetGroupByIdQuery(new GroupId(groupId)))
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }

    /// <summary>
    /// Gets the group with specified short name, if it exists.
    /// </summary>
    /// <param name="shortName">The group short name.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The group with specified identifier, if it exists.</returns>
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Groups.GetGroupByShortName)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroupByShortName([FromRoute] string shortName,
        CancellationToken token)
    {
        return await Result.Create(
                new GetGroupByShortNameQuery(shortName))
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }

    /// <summary>
    /// Gets groups with specified filter.
    /// </summary>
    /// <param name="pagination"></param>
    /// <param name="filter">The specified filter.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>Groups with specified filter.</returns>
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Groups.GetGroups)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroups([FromQuery] GetQueryPagination pagination, 
        [FromQuery] GetGroupsQueryFilter filter,
        CancellationToken token)
    {
        return await Result.Create(
                new GetGroupsQuery(pagination, filter))
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }

    /// <summary>
    /// Gets total groups count.
    /// </summary>
    /// <param name="token">The cancellation token.</param>
    /// <returns>Total groups count.</returns>
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Groups.GetGroupsCount)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroupsCount(
        CancellationToken token)
    {
        return await Result.Create(
                new GetGroupsCountQuery())
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }
    
    /// <summary>
    /// Creates a new group on the specified request.
    /// </summary>
    /// <param name="request">The specified create group request.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The identifier of the newly created group.</returns>
    [HttpPost(ApiRoutes.Groups.CreateGroup)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request, 
        CancellationToken token)
    {
        return await Result.Create(
                new CreateGroupCommand(new UserId(UserId),
                    request.Name,
                    request.ShortName,
                    request.Description))
            .Bind(command => Sender.Send(command, token))
            .Match(
                groupId => CreatedAtAction(nameof(GetGroupById), new { groupId }, groupId), 
                HandleFailure);
    }
    
    /// <summary>
    /// Updates the group with specified identifier, if it exists.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="request">The specified update group request.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpPut(ApiRoutes.Groups.UpdateGroup)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateGroup([FromRoute] Guid groupId, [FromBody] UpdateGroupRequest request, 
        CancellationToken token)
    {
        return await Result.Create(
                new UpdateGroupCommand(new GroupId(groupId),
                    request.Name,
                    request.ShortName,
                    request.Description))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }
    
    /// <summary>
    /// Removes the group with specified identifier, if it exists.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpDelete(ApiRoutes.Groups.RemoveGroup)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveGroup([FromRoute] Guid groupId,
        CancellationToken token)
    {
        return await Result.Create(
                new RemoveGroupCommand(new GroupId(groupId)))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }

    /// <summary>
    /// Gets group posts with specified filter.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="pagination"></param>
    /// <param name="filter">The specified filter.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>Group posts with specified filter.</returns>
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Groups.GetPosts)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPosts([FromRoute] Guid groupId, [FromQuery] GetQueryPagination pagination,
        [FromQuery] GetPostsQueryFilter filter,
        CancellationToken token)
    {
        return await Result.Create(
                new GetPostsQuery(new GroupId(groupId), pagination, filter))
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }
    
    /// <summary>
    /// Creates a new post in the specified group, if it exists.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="request">The specified create post request.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The identifier of the newly created post.</returns>
    [HttpPost(ApiRoutes.Groups.CreatePost)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePost([FromRoute] Guid groupId, [FromBody] CreatePostRequest request,
        CancellationToken token)
    {
        return await Result.Create(
                new CreatePostCommand(new GroupId(groupId),
                    request.Title,
                    request.Content,
                    request.Description))
            .Bind(command => Sender.Send(command, token))
            .Match(
                postId => CreatedAtAction("GetPostById", "Posts", new { postId }, postId),
                HandleFailure);
    }

    /// <summary>
    /// Creates a new subscription to a group by user identifier.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpPost(ApiRoutes.Groups.CreateSubscription)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateSubscription([FromRoute] Guid groupId,
        CancellationToken token)
    {
        return await Result.Create(
                new CreateSubscriptionCommand(
                    new UserId(UserId), 
                    new GroupId(groupId)))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }

    /// <summary>
    /// Removes the subscription from a group by user identifier.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpDelete(ApiRoutes.Groups.RemoveSubscription)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveSubscription([FromRoute] Guid groupId,
        CancellationToken token)
    {
        return await Result.Create(
                new RemoveSubscriptionCommand(
                    new UserId(UserId), 
                    new GroupId(groupId)))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }
}