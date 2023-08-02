using Microsoft.AspNetCore.Mvc;
using Scribble.Content.Models.Shared;

namespace Scribble.Content.Presentation.Extensions;

internal static class ResultExtensions
{
    internal static async Task<IActionResult> Match(
        this Task<Result> resultTask,
        Func<IActionResult> onSuccess,
        Func<Result, IActionResult> onFailure)
    {
        var result = await resultTask;

        return result.IsSuccess ? onSuccess() : onFailure(result);
    }
    
    internal static async Task<IActionResult> Match<TIn>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, IActionResult> onSuccess,
        Func<Result, IActionResult> onFailure)
    {
        var result = await resultTask;

        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }

    internal static async Task<Result<TOut>> Map<TIn, TOut>(
        this Task<Result<TIn>> resultTask,
        Func<TIn, TOut> mappingFunc)
    {
        var result = await resultTask;
        
        return result.IsSuccess
            ? Result.Success(mappingFunc(result.Value))
            : Result.Failure<TOut>(result.Errors);
    }
}