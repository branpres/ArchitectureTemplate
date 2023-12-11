namespace ArchitectureTemplate.Application.Domain.Aggregates.Projects.DomainEvents;

internal record ProjectDeletedDomainEvent(Project Project) : IDomainEvent;