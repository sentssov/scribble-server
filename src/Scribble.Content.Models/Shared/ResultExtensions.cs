namespace Scribble.Content.Models.Shared;

public static class ResultExtensions
{
    public static Result<T> Ensure<T>(this Result<T> result,
        Func<T, bool> predicate, Error error)
    {
        if (result.IsFailure)
        {
            return result;
        }

        return predicate(result.Value)
            ? result
            : Result.Failure<T>(error);
    }
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, 
        Func<TIn, TOut> mappingFunc)
    {
        return result.IsSuccess
            ? Result.Success(mappingFunc(result.Value))
            : Result.Failure<TOut>(result.Errors); 
    }

    public static async Task<Result<TOut>> Map<TIn, TOut>(this Task<Result<TIn>> resultTask,
        Func<TIn, TOut> mappingFunc)
    {
        return (await resultTask)
            .Map(mappingFunc);
    }

    /// <summary>
    /// Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
    /// </summary>
    /// <param name="result">The calling result.</param>
    /// <param name="func">The given select function.</param>
    /// <returns></returns>
    public static Result Bind(this Result result,
        Func<Result> func)
    {
        return result.IsFailure
            ? Result.Failure(result.Errors)
            : func();
    }
    
    /// <summary>
    /// Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
    /// </summary>
    /// <param name="result">The calling result.</param>
    /// <param name="func">The given select function.</param>
    /// <typeparam name="TOut">The output object.</typeparam>
    /// <returns></returns>
    public static Result<TOut> Bind<TOut>(this Result result,
        Func<Result<TOut>> func)
    {
        return result.IsFailure
            ? Result.Failure<TOut>(result.Errors)
            : func();
    }
    
    /// <summary>
    /// Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
    /// </summary>
    /// <param name="result">The calling result.</param>
    /// <param name="func">The given select function.</param>
    /// <typeparam name="TIn">The input object.</typeparam>
    /// <returns></returns>
    public static Result Bind<TIn>(this Result<TIn> result,
        Func<TIn, Result> func)
    {
        return result.IsFailure
            ? Result.Failure(result.Errors)
            : func(result.Value);
    }

    /// <summary>
    /// Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
    /// </summary>
    /// <param name="result">The calling result.</param>
    /// <param name="func">The given select function.</param>
    /// <typeparam name="TIn">The input object.</typeparam>
    /// <returns></returns>
    public static async Task<Result> Bind<TIn>(this Result<TIn> result,
        Func<TIn, Task<Result>> func)
    {
        return result.IsFailure
            ? Result.Failure(result.Errors)
            : await func(result.Value);
    }

    /// <summary>
    /// Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
    /// </summary>
    /// <param name="result">The calling result.</param>
    /// <param name="func">The given select function.</param>
    /// <typeparam name="TIn">The input object.</typeparam>
    /// <typeparam name="TOut">The output object.</typeparam>
    /// <returns></returns>
    public static Result<TOut> Bind<TIn, TOut>(this Result<TIn> result, 
        Func<TIn, Result<TOut>> func)
    {
        return result.IsFailure 
            ? Result.Failure<TOut>(result.Errors) 
            : func(result.Value);
    }

    /// <summary>
    /// Selects result from the return value of a given function. If the calling Result is a failure, a new failure result is returned instead.
    /// </summary>
    /// <param name="result">The calling result.</param>
    /// <param name="func">The given select function.</param>
    /// <typeparam name="TIn">The input object.</typeparam>
    /// <typeparam name="TOut">The output object.</typeparam>
    /// <returns></returns>
    public static async Task<Result<TOut>> Bind<TIn, TOut>(this Result<TIn> result,
        Func<TIn, Task<TOut>> func)
    {
        return result.IsFailure
            ? Result.Failure<TOut>(result.Errors)
            : await func(result.Value);
    }

    /// <summary>
    /// Executes the given action if the calling result is a success.
    /// </summary>
    /// <param name="result">The calling result.</param>
    /// <param name="action">The given action.</param>
    /// <returns>The calling result.</returns>
    public static Result<TIn> Tap<TIn>(this Result<TIn> result, 
        Action<TIn> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }
}