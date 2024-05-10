namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents;

public interface IDomainEventOutboxMessageHandler
{
    Task Handle(IDomainEvent domainEvent);
}