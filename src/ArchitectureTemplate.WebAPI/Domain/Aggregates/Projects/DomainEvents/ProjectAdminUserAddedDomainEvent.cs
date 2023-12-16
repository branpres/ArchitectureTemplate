namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents;

internal record ProjectAdminUserAddedDomainEvent(ProjectUser ProjectUser) : IDomainEvent;