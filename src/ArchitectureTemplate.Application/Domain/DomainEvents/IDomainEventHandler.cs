namespace ArchitectureTemplate.Application.Domain.DomainEvents;

internal interface IDomainEventHandler<T> where T : IDomainEvent
{
    Task Handle(T domainEvent);
}