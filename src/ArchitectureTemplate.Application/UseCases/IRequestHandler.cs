namespace ArchitectureTemplate.Application.UseCases;

public interface IRequestHandler<TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}

public interface IRequestHandler<TRequest>
{
    Task Handle(TRequest request, CancellationToken cancellationToken);
}