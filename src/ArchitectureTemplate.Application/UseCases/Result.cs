namespace ArchitectureTemplate.Application.UseCases;

public class Result<T>
{
    private readonly T? _value;
    private readonly Exception? _exception;

    public Result(T value)
    {
        _value = value;
        _exception = null;
        IsSuccess = true;
    }

    public Result(Exception exception)
    {
        _value = default;
        _exception = exception;
    }

    public bool IsSuccess { get; }

    public bool IsError => !IsSuccess;

    public TResult Match<TResult>(Func<T, TResult> success, Func<Exception, TResult> error)
        => IsSuccess ? success(_value!) : error(_exception!);
} 