namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents;

public record ProjectUserAddedDomainEvent(ProjectUser ProjectUser) : IDomainEvent;