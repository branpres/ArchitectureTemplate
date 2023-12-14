namespace ArchitectureTemplate.Application.UseCases;

public interface IRequestHandler<TRequest, TResponse> where TRequest : class
{
    Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IRequestHandler<TRequest> where TRequest : class
{
    Task<Result> Handle(TRequest request, CancellationToken cancellationToken);
}