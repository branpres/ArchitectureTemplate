namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents;

internal record ProjectUserAddedDomainEvent(ProjectUser ProjectUser) : IDomainEvent;