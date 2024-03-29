﻿namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents.Handlers;

public class WhenProjectDeletedDeleteScopePackages(TemplateDbContext templateDbContext) : IDomainEventHandler<ProjectDeleted>
{
    public async Task Handle(ProjectDeleted domainEvent)
    {
        var scopePackages = await templateDbContext.ScopePackage
            .Where(x => x.ProjectId == domainEvent.Project.ProjectId && !x.IsDeleted)
            .ToListAsync();
        scopePackages.ForEach(x => x.SoftDelete());

        Console.WriteLine("Scope Packages Deleted");
    }
}