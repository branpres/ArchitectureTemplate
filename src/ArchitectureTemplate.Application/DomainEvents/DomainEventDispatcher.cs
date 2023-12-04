namespace ArchitectureTemplate.Application.DomainEvents;

public interface IDomainEventDispatcher
{
    Task DispatchDomainEvents(List<DomainEventEntityBase> entities);
}

public class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task DispatchDomainEvents(List<DomainEventEntityBase> entities)
    {
        var domainEvents = entities.SelectMany(x => x.DomainEvents).ToList();
        await DispatchDomainEventsAsync(domainEvents);
    }

    private async Task DispatchDomainEventsAsync(List<IDomainEvent> domainEvents)
    {
        if (domainEvents.Count != 0)
        {
            foreach (var domainEvent in domainEvents)
            {
                var domainEventHandlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
                var handleDomainEventAsyncMethod = domainEventHandlerType.GetMethod("Handle");
                var domainEventHandlers = _serviceProvider.GetServices(domainEventHandlerType);
                if (domainEventHandlers != null && domainEventHandlers.Any())
                {
                    foreach (var domainEventHandler in domainEventHandlers)
                    {
                        if (domainEventHandler != null)
                        {
                            await handleDomainEventAsyncMethod?.Invoke((dynamic)domainEventHandler, new object[] { domainEvent });
                        }
                    }
                }
            }
        }
    }
}