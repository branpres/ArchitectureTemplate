namespace ArchitectureTemplate.Application.UseCases;

public interface IRequestHandler<TRequest, TResponse>
{
    Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IRequestHandler<TRequest>
{
    Task<Result> Handle(TRequest request, CancellationToken cancellationToken);
}