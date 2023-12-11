namespace ArchitectureTemplate.Application.Domain.Aggregates.Projects.DomainEvents;

internal record ProjectUserAddedDomainEvent(ProjectUser ProjectUser) : IDomainEvent;