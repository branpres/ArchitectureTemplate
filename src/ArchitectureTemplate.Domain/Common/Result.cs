namespace ArchitectureTemplate.Domain.Common;

public class Result<T>
{
    private readonly T? _value;
    private readonly ResultException? _exception;

    public Result(T value)
    {
        _value = value;
        _exception = null;
        IsSuccess = true;
    }

    public Result(ResultException exception)
    {
        _value = default;
        _exception = exception;
    }

    public bool IsSuccess { get; }

    public bool IsError => !IsSuccess;

    public TResult Match<TResult>(Func<T, TResult> success, Func<ResultException, TResult> error)
        => IsSuccess ? success(_value!) : error(_exception!);
} 