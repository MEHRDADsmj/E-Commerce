namespace Shared.Data;

public class Result
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }

    protected Result(bool isSuccess, string? errorMessage)
    {
        if (isSuccess && errorMessage != null) throw new InvalidOperationException();
        if (!isSuccess && errorMessage == null) throw new InvalidOperationException();
        
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result Success() => new Result(true, null);
    public static Result Failure(string error) => new Result(false, error);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(T value) : base(true, null)
    {
        Value = value;
    }

    private Result(string errorMessage) : base(false, errorMessage)
    {
        Value = default;
    }

    public static Result<T> Success(T value) => new Result<T>(value);
    public new static Result<T> Failure(string error) => new Result<T>(error);
}