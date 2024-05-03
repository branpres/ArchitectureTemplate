namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents;

public interface IDomainEventHandler
{
    Task Handle(IDomainEvent domainEvent);
}