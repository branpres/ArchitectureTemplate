namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents.Projects.ProjectAdminUserAdded;

public record ProjectAdminUserAdded(ProjectUser ProjectUser) : IDomainEvent;