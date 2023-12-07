namespace ArchitectureTemplate.Application.Domain.Aggregates.Projects.DomainEvents;

public record ProjectAdminUserAddedDomainEvent(ProjectUser ProjectUser) : IDomainEvent;