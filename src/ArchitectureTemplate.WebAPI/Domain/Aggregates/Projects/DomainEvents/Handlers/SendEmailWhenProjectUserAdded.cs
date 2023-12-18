namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents.Handlers;

public class SendEmailWhenProjectUserAdded : IDomainEventOutboxMessageHandler<ProjectUserAdded>
{
    public Task Handle(ProjectUserAdded domainEvent)
    {
        Console.WriteLine($"Email sent to project user. {DateTime.Now}");

        return Task.CompletedTask;
    }
}