namespace ArchitectureTemplate.WebAPI.BackgroundServices.OutboxMessageProcessing.DomainEventOutboxMessageHandlers;

public interface IDomainEventOutboxMessageHandler<T> where T : IDomainEvent
{
    Task Handle(T domainEvent);
}