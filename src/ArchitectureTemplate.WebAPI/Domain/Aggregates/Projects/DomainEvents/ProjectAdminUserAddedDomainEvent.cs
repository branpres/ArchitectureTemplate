namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents;

public record ProjectAdminUserAddedDomainEvent(ProjectUser ProjectUser) : IDomainEvent;