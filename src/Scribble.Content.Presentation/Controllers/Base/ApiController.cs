using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using Scribble.Content.Models.Entities;
using Scribble.Content.Models.Shared;
using Scribble.Content.Models.ValueObjects;

namespace Scribble.Content.Presentation.Controllers.Base;

/// <inheritdoc />
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;
    
    protected readonly IMapper Mapper;

    /// <inheritdoc />
    protected ApiController(
        ISender sender, IMapper mapper)
    {
        Sender = sender;
        Mapper = mapper;
    }

    /// <summary>
    /// Current user id.
    /// </summary>
    protected Guid UserId
    {
        get
        {
            var sub = User
                .GetClaim(OpenIddictConstants.Claims.Subject);

            return sub is null 
                ? Guid.Empty
                : Guid.Parse(sub);
        }
    }
    
    protected IActionResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult => 
                BadRequest(
                    CreateProblemDetails(
                        "Validation Error", StatusCodes.Status400BadRequest,
                        result.Errors.First(),
                        validationResult.Errors)),
            _ =>
                BadRequest(CreateProblemDetails(
                    "Bad Request", StatusCodes.Status400BadRequest,
                        result.Errors.First()))
        };

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}