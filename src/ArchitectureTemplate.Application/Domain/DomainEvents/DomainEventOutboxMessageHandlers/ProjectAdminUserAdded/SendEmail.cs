﻿namespace ArchitectureTemplate.Application.Domain.DomainEvents.DomainEventOutboxMessageHandlers.ProjectAdminUserAdded;

internal class SendEmail : IDomainEventOutboxMessageHandler<ProjectAdminUserAddedDomainEvent>
{
    public Task Handle(ProjectAdminUserAddedDomainEvent domainEvent)
    {
        Console.WriteLine($"Email sent to project admin user. {DateTime.Now}");

        return Task.CompletedTask;
    }
}