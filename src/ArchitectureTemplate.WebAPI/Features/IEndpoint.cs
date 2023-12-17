namespace ArchitectureTemplate.WebAPI.Features;

public interface IEndpoint
{
    IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder);
}