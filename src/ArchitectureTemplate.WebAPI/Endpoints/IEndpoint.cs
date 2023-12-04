namespace ArchitectureTemplate.WebAPI.Endpoints;

public interface IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder);
}