namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents;

public record ProjectCreated(Project Project) : IDomainEvent;