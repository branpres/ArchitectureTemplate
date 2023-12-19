namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents.Handlers;

public class WhenProjectAdminUserAddedSendEmail : IDomainEventOutboxMessageHandler<ProjectAdminUserAdded>
{
    public Task Handle(ProjectAdminUserAdded domainEvent)
    {
        Console.WriteLine($"Email sent to project admin user. {DateTime.Now}");

        return Task.CompletedTask;
    }
}