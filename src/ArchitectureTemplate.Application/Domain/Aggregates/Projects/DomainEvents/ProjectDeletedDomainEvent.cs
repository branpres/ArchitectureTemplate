namespace ArchitectureTemplate.Application.Domain.Aggregates.Projects.DomainEvents;

public record ProjectDeletedDomainEvent(Project Project) : IDomainEvent;