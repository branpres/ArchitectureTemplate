namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents;

public static class DomainEventDispatcher
{
    public static async Task DispatchDomainEvents(TemplateDbContext templateDbContext)
    {
        var handlers = GetHandlers(templateDbContext);
        var entities = GetEntitiesWithDomainEvents(templateDbContext);

        var domainEvents = entities.SelectMany(x => x.DomainEvents).ToList();
        entities.ForEach(x => x.ClearDomainEvents());

        if (domainEvents.Count != 0)
        {
            foreach (var domainEvent in domainEvents)
            {
                if (handlers.TryGetValue(domainEvent.GetType(), out var domainEventhandlers))
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

    private static Dictionary<Type, List<IDomainEventHandler>> GetHandlers(TemplateDbContext templateDbContext)
    {
        return new Dictionary<Type, List<IDomainEventHandler>>
        {
            {
                typeof(ProjectCreated),
                [
                    new WhenProjectCreatedCreateBillOfMaterials(templateDbContext),
                    new WhenProjectCreatedCreateInitialScopePackage(templateDbContext)
                ]
            },
            {
                typeof(ProjectDeleted),
                [
                    new WhenProjectDeletedDeleteBillOfMaterials(templateDbContext),
                    new WhenProjectDeletedDeleteScopePackages(templateDbContext)
                ]
            }
        };
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