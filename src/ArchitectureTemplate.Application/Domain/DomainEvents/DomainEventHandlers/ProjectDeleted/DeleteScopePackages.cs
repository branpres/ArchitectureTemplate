﻿namespace ArchitectureTemplate.Application.Domain.DomainEvents.DomainEventHandlers.ProjectDeleted;

internal class DeleteScopePackages(TemplateDbContext templateDbContext) : IDomainEventHandler<ProjectDeletedDomainEvent>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task Handle(ProjectDeletedDomainEvent domainEvent)
    {
        var scopePackages = await _templateDbContext.ScopePackage
            .Where(x => x.ProjectId == domainEvent.Project.ProjectId && !x.IsDeleted)
            .ToListAsync();
        scopePackages.ForEach(x => x.SoftDelete());

        Console.WriteLine("Scope Packages Deleted");
    }
}