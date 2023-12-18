namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents;

public record ProjectAdminUserAdded(ProjectUser ProjectUser) : IDomainEvent;