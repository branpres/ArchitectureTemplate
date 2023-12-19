namespace ArchitectureTemplate.WebAPI.Features;

public interface IRequestHandler<in TRequest, TResponse>
{
    Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IRequestHandler<in TRequest>
{
    Task<Result> Handle(TRequest request, CancellationToken cancellationToken);
}