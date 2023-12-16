namespace ArchitectureTemplate.WebAPI.BackgroundServices.OutboxMessageProcessing.DomainEventOutboxMessageHandlers;

public static class OutboxMessageHandlerExtensions
{
    public static IServiceCollection AddDomainEventOutboxMessageHandling(this IServiceCollection services)
    {
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