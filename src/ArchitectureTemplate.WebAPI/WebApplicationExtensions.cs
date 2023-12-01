namespace ArchitectureTemplate.WebAPI;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpoints = DiscoverEndpoints();
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }
        return app;
    }

    private static IEnumerable<IEndpoint> DiscoverEndpoints()
    {
        return typeof(IEndpoint).Assembly
        .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IEndpoint)))
            .Select(Activator.CreateInstance)
            .Cast<IEndpoint>();
    }
}