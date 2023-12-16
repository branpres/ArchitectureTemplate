namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents;

internal record ProjectCreatedDomainEvent(Project Project) : IDomainEvent;