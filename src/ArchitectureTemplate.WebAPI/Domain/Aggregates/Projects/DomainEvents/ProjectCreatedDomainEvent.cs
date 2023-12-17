namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents;

public record ProjectCreatedDomainEvent(Project Project) : IDomainEvent;