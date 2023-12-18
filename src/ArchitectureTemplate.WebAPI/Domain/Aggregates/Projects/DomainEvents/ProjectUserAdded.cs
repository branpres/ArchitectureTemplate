namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents;

public record ProjectUserAdded(ProjectUser ProjectUser) : IDomainEvent;