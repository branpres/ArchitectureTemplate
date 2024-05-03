namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents;

public class DomainEventDispatcher
{
    private readonly TemplateDbContext _templateDbContext;
    private readonly Dictionary<Type, List<IDomainEventHandler>> _handlers = [];

    public DomainEventDispatcher(TemplateDbContext templateDbContext)
    {
        _templateDbContext = templateDbContext;

        AddHandlers();
    }    

    public async Task DispatchDomainEvents()
    {
        var entities = GetEntitiesWithDomainEvents();
        var domainEvents = entities.SelectMany(x => x.DomainEvents).ToList();
        if (domainEvents.Count != 0)
        {
            foreach (var domainEvent in domainEvents)
            {
                foreach (var handler in _handlers[domainEvent.GetType()])
                {
                    await handler.Handle(domainEvent);
                }
            }

            await PersistDomainEvents(domainEvents);
        }
    }

    private void AddHandlers()
    {
        _handlers.Add(typeof(ProjectCreated),
        [
            new WhenProjectCreatedCreateBillOfMaterials(_templateDbContext),
            new WhenProjectCreatedCreateInitialScopePackage(_templateDbContext)
        ]);

        _handlers.Add(typeof(ProjectDeleted),
        [
            new WhenProjectDeletedDeleteBillOfMaterials(_templateDbContext),
            new WhenProjectDeletedDeleteScopePackages(_templateDbContext)
        ]);
    }

    private List<DomainEventEntityBase> GetEntitiesWithDomainEvents()
    {
        return _templateDbContext.ChangeTracker.Entries<DomainEventEntityBase>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .Select(x => x.Entity)
            .ToList();
    }

    private async Task PersistDomainEvents(List<IDomainEvent> domainEvents)
    {
        await _templateDbContext.OutboxMessage.AddRangeAsync(
            domainEvents
            .Select(x =>
                new OutboxMessage(
                    x.GetType().Name,
                    JsonConvert.SerializeObject(x, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }))));
    }
}