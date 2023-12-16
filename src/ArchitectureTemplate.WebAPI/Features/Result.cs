namespace ArchitectureTemplate.WebAPI.Features;

public class Result<T>
{
    private readonly T? _value;
    private readonly ResultProblem? _problem = null;

    public Result(T value)
    {
        _value = value;
        IsSuccess = true;
    }

    public Result(ResultProblem problem)
    {
        _value = default;
        _problem = problem;
    }

    public bool IsSuccess { get; }

    public bool IsError => !IsSuccess;

    public TResult Match<TResult>(Func<T, TResult> success, Func<ResultProblem, TResult> error)
        => IsSuccess ? success(_value!) : error(_problem!);
}

public class Result
{
    private readonly ResultProblem? _problem = null;

    public Result()
    {
        IsSuccess = true;
    }

    public Result(ResultProblem problem)
    {
        _problem = problem;
    }

    public bool IsSuccess { get; }

    public bool IsError => !IsSuccess;

    public TResult Match<TResult>(Func<TResult> success, Func<ResultProblem, TResult> error)
        => IsSuccess ? success() : error(_problem!);
}