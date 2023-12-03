namespace ArchitectureTemplate.Application.UseCases;

public interface IRequestHandler<TRequest, TResponse>
{
    Task<Result<TResponse?>> Handle(TRequest request, CancellationToken cancellationToken);
}