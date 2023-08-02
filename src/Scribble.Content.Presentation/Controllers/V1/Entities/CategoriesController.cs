using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Application.Abstractions;
using Scribble.Content.Application.Categories.Commands.CreateCategory;
using Scribble.Content.Application.Categories.Commands.ExcludeGroup;
using Scribble.Content.Application.Categories.Commands.IncludeGroup;
using Scribble.Content.Application.Categories.Commands.RemoveCategory;
using Scribble.Content.Application.Categories.Commands.UpdateCategory;
using Scribble.Content.Application.Categories.Queries.GetCategories;
using Scribble.Content.Application.Categories.Queries.GetCategoryById;
using Scribble.Content.Application.Groups.Queries.GetGroups;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Shared;
using Scribble.Content.Presentation.Contracts;
using Scribble.Content.Presentation.Contracts.Categories.Requests;
using Scribble.Content.Presentation.Controllers.Base;
using Scribble.Content.Presentation.Extensions;
using Scribble.Identity.Authorization;

namespace Scribble.Content.Presentation.Controllers.V1.Entities;

/// <summary>
/// Represents the categories controller.
/// </summary>
[ApiController]
[Authorize(Policy = DefaultPolicies.AdministratorOnly.Name)]
public class CategoriesController : ApiController
{
    /// <inheritdoc />
    public CategoriesController(ISender sender, IMapper mapper)
        : base(sender, mapper)
    {
    }

    /// <summary>
    /// Gets the category with specified identifier, if it exists.
    /// </summary>
    /// <param name="categoryId">The category id.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The category with specified identifier, if it exists.</returns>
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Categories.GetCategoryById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid categoryId,
        CancellationToken token)
    {
        return await Result.Create(
                new GetCategoryByIdQuery(new CategoryId(categoryId)))
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }

    /// <summary>
    /// Gets categories with specified filter.
    /// </summary>
    /// <param name="filter">The specified filter.</param>
    /// <param name="pagination">The pagination parameters.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>Categories with specified filter.</returns>
    [AllowAnonymous]
    [HttpGet(ApiRoutes.Categories.GetCategories)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories([FromQuery] GetQueryPagination pagination, 
        [FromQuery] GetCategoriesQueryFilter filter,
        CancellationToken token)
    {
        return await Result.Create(
                new GetCategoriesQuery(pagination, filter))
            .Bind(query => Sender.Send(query, token))
            .Match(Ok, HandleFailure);
    }

    /// <summary>
    /// Creates a new category on the specified request.
    /// </summary>
    /// <param name="request">The create category request.</param>
    /// <param name="token">The cancellation token.</param>
    /// <returns>The identifier of the newly created category.</returns>
    [HttpPost(ApiRoutes.Categories.CreateCategory)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request,
        CancellationToken token)
    {
        return await Result.Create(
                new CreateCategoryCommand(new UserId(UserId),
                    request.Name))
            .Bind(command => Sender.Send(command, token))
            .Match(
                categoryId => CreatedAtAction(nameof(GetCategoryById), new { categoryId }, categoryId), 
                HandleFailure);
    }
    
    /// <summary>
    /// Updates the category with specified identifier on the specified request, if it exists.
    /// </summary>
    /// <param name="categoryId">The category id.</param>
    /// <param name="request">The update category request.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpPut(ApiRoutes.Categories.UpdateCategory)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid categoryId, [FromBody] UpdateCategoryRequest request,
        CancellationToken token)
    {
        return await Result.Create(
                new UpdateCategoryCommand(new CategoryId(categoryId),
                    request.Name))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }

    /// <summary>
    /// Removes the category with specified identifier, if it exists.
    /// </summary>
    /// <param name="categoryId">The category id.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpDelete(ApiRoutes.Categories.RemoveCategory)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveCategory([FromRoute] Guid categoryId, 
        CancellationToken token)
    {
        return await Result.Create(
                new RemoveCategoryCommand(new CategoryId(categoryId)))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }
    
    /// <summary>
    /// Adds the group with specified identifier into the category with specified identifier.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpPost(ApiRoutes.Categories.IncludeGroup)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> IncludeGroup([FromRoute] Guid categoryId, [FromQuery] Guid groupId, 
        CancellationToken token)
    {
        return await Result.Create(
                new IncludeGroupCommand(
                    new GroupId(groupId), 
                    new CategoryId(categoryId)))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }
    
    /// <summary>
    /// Removes the group with specified identifier from the category with specified identifier.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <param name="token">The cancellation token.</param>
    [HttpDelete(ApiRoutes.Categories.ExcludeGroup)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ExcludeGroup([FromRoute] Guid categoryId, [FromQuery] Guid groupId,
        CancellationToken token)
    {
        return await Result.Create(
                new ExcludeGroupCommand(
                    new GroupId(groupId), 
                    new CategoryId(categoryId)))
            .Bind(command => Sender.Send(command, token))
            .Match(NoContent, HandleFailure);
    }
}