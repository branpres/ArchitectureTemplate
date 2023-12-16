namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents;

internal record ProjectDeletedDomainEvent(Project Project) : IDomainEvent;