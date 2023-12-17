namespace ArchitectureTemplate.WebAPI.BackgroundServices.OutboxMessageProcessing.DomainEventOutboxMessageHandlers.ProjectAdminUserAdded;

public class SendEmail : IDomainEventOutboxMessageHandler<ProjectAdminUserAddedDomainEvent>
{
    public Task Handle(ProjectAdminUserAddedDomainEvent domainEvent)
    {
        Console.WriteLine($"Email sent to project admin user. {DateTime.Now}");

        return Task.CompletedTask;
    }
}