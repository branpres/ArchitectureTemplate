﻿namespace ArchitectureTemplate.Domain.DomainEvents;

public interface IDomainEventHandler<T> where T : IDomainEvent
{
    Task Handle(T domainEvent);
}