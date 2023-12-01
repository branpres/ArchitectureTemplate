namespace ArchitectureTemplate.WebAPI;

public interface IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder); 
}