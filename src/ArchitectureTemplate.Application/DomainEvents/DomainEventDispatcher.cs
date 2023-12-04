namespace ArchitectureTemplate.Application.DomainEvents;

public interface IDomainEventDispatcher
{
    Task DispatchDomainEvents(TemplateDbContext templateDbContext);
}

public class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task DispatchDomainEvents(TemplateDbContext templateDbContext)
    {
        var entities = GetEntitiesWithDomainEvents(templateDbContext);
        var domainEvents = entities.SelectMany(x => x.DomainEvents).ToList();
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

    private static List<DomainEventEntityBase> GetEntitiesWithDomainEvents(DbContext dbContext)
    {
        return dbContext.ChangeTracker.Entries<DomainEventEntityBase>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .Select(x => x.Entity)
            .ToList();
    }
}