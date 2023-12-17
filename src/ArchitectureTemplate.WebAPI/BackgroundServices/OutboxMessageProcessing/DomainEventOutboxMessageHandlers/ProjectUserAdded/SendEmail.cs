namespace ArchitectureTemplate.WebAPI.BackgroundServices.OutboxMessageProcessing.DomainEventOutboxMessageHandlers.ProjectUserAdded;

public class SendEmail : IDomainEventOutboxMessageHandler<ProjectUserAddedDomainEvent>
{
    public Task Handle(ProjectUserAddedDomainEvent domainEvent)
    {
        Console.WriteLine($"Email sent to project user. {DateTime.Now}");

        return Task.CompletedTask;
    }
}