namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents.Projects.ProjectAdminUserAdded.DomainEventOutboxMessageHandlers;

public class SendEmail : IDomainEventOutboxMessageHandler<ProjectAdminUserAdded>
{
    public Task Handle(ProjectAdminUserAdded domainEvent)
    {
        Console.WriteLine($"Email sent to project admin user. {DateTime.Now}");

        return Task.CompletedTask;
    }
}