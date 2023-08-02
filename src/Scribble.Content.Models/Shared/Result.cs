namespace Scribble.Content.Models.Shared;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
                throw new InvalidOperationException();
            case false when error == Error.None:
                throw new InvalidOperationException();
            default:
                IsSuccess = isSuccess;
                Errors = new []{ error };
                break;
        }
    }
    
    protected internal Result(bool isSuccess, Error[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error[] Errors { get; }

    public static Result Success() => 
        new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) => 
        new(value, true, Error.None);

    public static Result Failure(Error error) => 
        new(false, error);
    
    public static Result Failure(Error[] errors) => 
        new(false, errors);

    public static Result<TValue> Failure<TValue>(Error error) => 
        new(default, false, error);
    
    public static Result<TValue> Failure<TValue>(Error[] errors) => 
        new(default, false, errors);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static Result<T> Ensure<T>(T value, 
        Func<T, bool> predicate, Error error)
    {
        return predicate(value)
            ? Success(value)
            : Failure<T>(error);
    }

    public static Result<T> Ensure<T>(T value,
        params (Func<T, bool> predicate, Error error)[] functions)
    {
        var results = new List<Result<T>>();
        
        foreach (var (predicate, error) in functions)
        {
            results.Add(Ensure(value, predicate, error));
        }

        return Combine(results.ToArray());
    }
    
    public static Result Combine(params Result[] results)
    {
        if (results.Any(x => x.IsFailure))
        {
            return Failure(
                results.SelectMany(x => x.Errors)
                    .Distinct()
                    .ToArray());
        }

        return Success();
    }

    public static Result<T> Combine<T>(params Result<T>[] results)
    {
        if (results.Any(x => x.IsFailure))
        {
            return Failure<T>(
                results.SelectMany(x => x.Errors)
                    .Where(e => e != Error.None)
                    .Distinct()
                    .ToArray());
        }

        return Success(results[0].Value);
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;
    protected internal Result(TValue? value, bool isSuccess, Error error) 
        : base(isSuccess, error) =>
        _value = value;
    
    protected internal Result(TValue? value, bool isSuccess, Error[] errors) 
        : base(isSuccess, errors) =>
        _value = value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}