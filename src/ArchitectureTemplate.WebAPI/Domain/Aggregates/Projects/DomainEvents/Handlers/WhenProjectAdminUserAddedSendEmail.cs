namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents.Handlers;

public class WhenProjectAdminUserAddedSendEmail : IDomainEventOutboxMessageHandler
{
    public Task Handle(IDomainEvent domainEvent)
    {
        if (domainEvent is ProjectAdminUserAdded)
        {
            Console.WriteLine($"Email sent to project admin user. {DateTime.Now}");            
        }

        return Task.CompletedTask;
    }
}