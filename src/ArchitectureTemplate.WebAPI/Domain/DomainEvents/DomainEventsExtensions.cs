namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents;

public static class DomainEventsExtensions
{
    public static IServiceCollection AddDomainEventHandling(this IServiceCollection services)
    {
        services.AddScoped<DomainEventDispatcher>();

        typeof(IDomainEventOutboxMessageHandler<>)
            .Assembly
            .GetTypes()
            .Where(x => x.GetInterfaces()
            .Where(x => x.IsGenericType).Any(x => x.GetGenericTypeDefinition() == typeof(IDomainEventOutboxMessageHandler<>)) && !x.IsAbstract && !x.IsInterface)
            .ToList()
            .ForEach(type =>
            {
                var serviceType = type.GetInterfaces().First(x => x.GetGenericTypeDefinition() == typeof(IDomainEventOutboxMessageHandler<>));
                services.AddScoped(serviceType, type);
            });

        return services;
    }
}