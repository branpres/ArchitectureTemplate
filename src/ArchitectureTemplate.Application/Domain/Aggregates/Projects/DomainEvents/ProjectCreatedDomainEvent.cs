namespace ArchitectureTemplate.Application.Domain.Aggregates.Projects.DomainEvents;

internal record ProjectCreatedDomainEvent(Project Project) : IDomainEvent;