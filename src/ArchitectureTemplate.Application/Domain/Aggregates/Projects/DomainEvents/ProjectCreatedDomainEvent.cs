namespace ArchitectureTemplate.Application.Domain.Aggregates.Projects.DomainEvents;

public record ProjectCreatedDomainEvent(Project Project) : IDomainEvent;