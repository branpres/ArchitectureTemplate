namespace ArchitectureTemplate.Application.DomainEvents.DomainEventOutboxMessageHandlers.ProjectUserAdded;

internal class SendEmail : IDomainEventOutboxMessageHandler<ProjectUserAddedDomainEvent>
{
    public Task Handle(ProjectUserAddedDomainEvent domainEvent)
    {
        Console.WriteLine($"Email sent to project user. {DateTime.Now}");

        return Task.CompletedTask;
    }
}