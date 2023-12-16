namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents;

internal interface IDomainEventHandler<T> where T : IDomainEvent
{
    Task Handle(T domainEvent);
}