namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents.Handlers;

public class WhenProjectDeletedDeleteScopePackages(TemplateDbContext templateDbContext) : IDomainEventHandler
{
    public async Task Handle(IDomainEvent domainEvent)
    {
        if (domainEvent is ProjectDeleted projectDeleted)
        {
            var scopePackages = await templateDbContext.ScopePackage
            .Where(x => x.ProjectId == projectDeleted.Project.ProjectId && !x.IsDeleted)
            .ToListAsync();
            scopePackages.ForEach(x => x.SoftDelete());

            Console.WriteLine("Scope Packages Deleted");
        }
    }
}