namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents;

public class DomainEventDispatcher
{
    private readonly Dictionary<Type, List<IDomainEventHandler>> _handlers = [];

    public async Task DispatchDomainEvents(TemplateDbContext templateDbContext)
    {
        AddHandlers(templateDbContext);

        var entities = GetEntitiesWithDomainEvents(templateDbContext);
        var domainEvents = entities.SelectMany(x => x.DomainEvents).ToList();
        if (domainEvents.Count != 0)
        {
            foreach (var domainEvent in domainEvents)
            {
                if (_handlers.TryGetValue(domainEvent.GetType(), out var domainEventhandlers))
                {
                    foreach (var handler in domainEventhandlers)
                    {
                        await handler.Handle(domainEvent);
                    }
                }                
            }

            await PersistDomainEvents(templateDbContext, domainEvents);
        }
    }

    private void AddHandlers(TemplateDbContext templateDbContext)
    {
        _handlers.Add(typeof(ProjectCreated),
        [
            new WhenProjectCreatedCreateBillOfMaterials(templateDbContext),
            new WhenProjectCreatedCreateInitialScopePackage(templateDbContext)
        ]);

        _handlers.Add(typeof(ProjectDeleted),
        [
            new WhenProjectDeletedDeleteBillOfMaterials(templateDbContext),
            new WhenProjectDeletedDeleteScopePackages(templateDbContext)
        ]);
    }

    private static List<DomainEventEntityBase> GetEntitiesWithDomainEvents(TemplateDbContext templateDbContext)
    {
        return templateDbContext.ChangeTracker.Entries<DomainEventEntityBase>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .Select(x => x.Entity)
            .ToList();
    }

    private static async Task PersistDomainEvents(TemplateDbContext templateDbContext, List<IDomainEvent> domainEvents)
    {
        await templateDbContext.OutboxMessage.AddRangeAsync(
            domainEvents
            .Select(x =>
                new OutboxMessage(
                    x.GetType().Name,
                    JsonConvert.SerializeObject(x, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }))));
    }
}