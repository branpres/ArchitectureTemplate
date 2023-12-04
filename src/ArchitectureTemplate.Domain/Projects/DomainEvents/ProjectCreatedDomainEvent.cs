namespace ArchitectureTemplate.Domain.Projects.DomainEvents;

public record ProjectCreatedDomainEvent(Project Project) : IDomainEvent;