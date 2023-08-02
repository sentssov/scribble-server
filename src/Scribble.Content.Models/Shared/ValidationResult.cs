namespace Scribble.Content.Models.Shared;

public class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(false, errors) { }
    
    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}

public class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(default, false, errors) { }
    
    public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
}