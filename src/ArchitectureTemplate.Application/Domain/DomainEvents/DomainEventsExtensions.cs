namespace ArchitectureTemplate.Application.Domain.DomainEvents;

public static class DomainEventsExtensions
{
    public static IServiceCollection AddDomainEvents(this IServiceCollection services)
    {
        services.AddScoped<DomainEventDispatcher>();

        typeof(IDomainEventHandler<>)
            .Assembly
            .GetTypes()
            .Where(x => x.GetInterfaces()
            .Where(x => x.IsGenericType).Any(x => x.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>)) && !x.IsAbstract && !x.IsInterface)
            .ToList()
            .ForEach(type =>
            {
                var serviceType = type.GetInterfaces().First(x => x.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>));
                services.AddScoped(serviceType, type);
            });

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