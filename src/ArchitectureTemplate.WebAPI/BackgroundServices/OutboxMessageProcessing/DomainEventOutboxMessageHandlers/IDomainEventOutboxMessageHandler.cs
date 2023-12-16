namespace ArchitectureTemplate.WebAPI.BackgroundServices.OutboxMessageProcessing.DomainEventOutboxMessageHandlers;

internal interface IDomainEventOutboxMessageHandler<T> where T : IDomainEvent
{
    Task Handle(T domainEvent);
}