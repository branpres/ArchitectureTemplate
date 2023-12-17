namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents.Projects.ProjectUserAdded.DomainEventOutboxMessageHandlers;

public class SendEmail : IDomainEventOutboxMessageHandler<ProjectUserAdded>
{
    public Task Handle(ProjectUserAdded domainEvent)
    {
        Console.WriteLine($"Email sent to project user. {DateTime.Now}");

        return Task.CompletedTask;
    }
}