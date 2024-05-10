namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents.Handlers;

public class WhenProjectUserAddedSendEmail : IDomainEventOutboxMessageHandler
{
    public Task Handle(IDomainEvent domainEvent)
    {
        if (domainEvent is ProjectUserAdded)
        {
            Console.WriteLine($"Email sent to project user. {DateTime.Now}");
        }            

        return Task.CompletedTask;
    }
}