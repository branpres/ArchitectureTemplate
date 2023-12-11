namespace ArchitectureTemplate.Application.Domain.DomainEvents;

internal abstract class DomainEventEntityBase
{
    [NotMapped]
    public List<IDomainEvent> DomainEvents { get; private set; } = [];

    public void RegisterDomainEvent(IDomainEvent domainEvent)
    {
        DomainEvents.Add(domainEvent);
    }
}