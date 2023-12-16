namespace ArchitectureTemplate.WebAPI.Features;

public interface IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder);
}