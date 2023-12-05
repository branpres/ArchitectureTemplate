namespace ArchitectureTemplate.Application.UseCases;

public static class RequestHandlerExtensions
{
    public static void AddRequestHandlers(this IServiceCollection services)
    {
        typeof(IRequestHandler<,>)
            .Assembly
            .GetTypes()
            .Where(x => x.GetInterfaces()
            .Where(x => x.IsGenericType).Any(x => x.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)) && !x.IsAbstract && !x.IsInterface)
            .ToList()
            .ForEach(type =>
            {
                var serviceType = type.GetInterfaces().First(x => x.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
                services.AddScoped(serviceType, type);
            });

        typeof(IRequestHandler<>)
            .Assembly
            .GetTypes()
            .Where(x => x.GetInterfaces()
            .Where(x => x.IsGenericType).Any(x => x.GetGenericTypeDefinition() == typeof(IRequestHandler<>)) && !x.IsAbstract && !x.IsInterface)
            .ToList()
            .ForEach(type =>
            {
                var serviceType = type.GetInterfaces().First(x => x.GetGenericTypeDefinition() == typeof(IRequestHandler<>));
                services.AddScoped(serviceType, type);
            });
    }
}