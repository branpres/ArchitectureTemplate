namespace ArchitectureTemplate.Application.Domain.Aggregates.Projects.DomainEvents;

internal record ProjectAdminUserAddedDomainEvent(ProjectUser ProjectUser) : IDomainEvent;