namespace ArchitectureTemplate.Application.DomainEvents;

public interface IDomainEventOutboxMessageHandler<T> where T : IDomainEvent
{
    Task Handle(T domainEvent);
}