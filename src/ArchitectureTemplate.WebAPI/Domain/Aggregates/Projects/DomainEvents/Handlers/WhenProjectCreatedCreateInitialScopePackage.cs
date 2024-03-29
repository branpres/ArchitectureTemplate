﻿namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents.Handlers;

public class WhenProjectCreatedCreateInitialScopePackage(TemplateDbContext templateDbContext) : IDomainEventHandler<ProjectCreated>
{
    public async Task Handle(ProjectCreated domainEvent)
    {
        var scopePackage = new ScopePackage
        {
            ProjectId = domainEvent.Project.ProjectId
        };

        await templateDbContext.ScopePackage.AddAsync(scopePackage);

        Console.WriteLine("Scope Package Created");
    }
}