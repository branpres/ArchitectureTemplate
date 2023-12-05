﻿namespace ArchitectureTemplate.Domain.Common.DomainEvents;

public abstract class DomainEventEntityBase
{
    [NotMapped]
    public List<IDomainEvent> DomainEvents { get; private set; } = [];

    public void RegisterDomainEvent(IDomainEvent domainEvent)
    {
        DomainEvents.Add(domainEvent);
    }
}