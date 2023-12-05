namespace ArchitectureTemplate.Domain.Aggregates.Projects.DomainEvents;

public record ProjectUserAddedDomainEvent(ProjectUser ProjectUser) : IDomainEvent;