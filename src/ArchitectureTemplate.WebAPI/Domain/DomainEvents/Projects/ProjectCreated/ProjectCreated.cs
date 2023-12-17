namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents.Projects.ProjectCreated;

public record ProjectCreated(Project Project) : IDomainEvent;