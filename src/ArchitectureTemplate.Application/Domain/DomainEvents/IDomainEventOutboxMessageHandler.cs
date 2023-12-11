namespace ArchitectureTemplate.Application.Domain.DomainEvents;

internal interface IDomainEventOutboxMessageHandler<T> where T : IDomainEvent
{
    Task Handle(T domainEvent);
}