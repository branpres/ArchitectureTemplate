namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents.Projects.ProjectDeleted;

public record ProjectDeleted(Project Project) : IDomainEvent;