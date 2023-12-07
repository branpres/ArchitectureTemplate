namespace ArchitectureTemplate.Application.Domain.DomainEvents;

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
                var handleMethod = domainEventHandlerType.GetMethod("Handle");
                var domainEventHandlers = _serviceProvider.GetServices(domainEventHandlerType);
                if (domainEventHandlers != null && domainEventHandlers.Any())
                {
                    foreach (var domainEventHandler in domainEventHandlers)
                    {
                        if (domainEventHandler != null)
                        {
                            await handleMethod?.Invoke((dynamic)domainEventHandler, new object[] { domainEvent });
                        }
                    }
                }
            }

            await PersistDomainEvents(templateDbContext, domainEvents);
        }
    }

    private static List<DomainEventEntityBase> GetEntitiesWithDomainEvents(TemplateDbContext templateDbContext)
    {
        return templateDbContext.ChangeTracker.Entries<DomainEventEntityBase>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .Select(x => x.Entity)
            .ToList();
    }

    private async static Task PersistDomainEvents(TemplateDbContext templateDbContext, List<IDomainEvent> domainEvents)
    {
        await templateDbContext.OutboxMessage.AddRangeAsync(
            domainEvents
            .Select(x =>
                new OutboxMessage(
                    x.GetType().Name,
                    JsonConvert.SerializeObject(x, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }))));
    }
}