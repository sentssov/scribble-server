using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Scribble.Content.Web.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => 
        _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exp)
        {
            _logger.LogError(exp, exp.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var json = JsonConvert.SerializeObject(
                new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "Server error",
                Title = "Server error",
                Detail = "An internal server error has occurred."
            });

            await context.Response.WriteAsync(json);

            context.Response.ContentType = "application/json";
        }
    }
}