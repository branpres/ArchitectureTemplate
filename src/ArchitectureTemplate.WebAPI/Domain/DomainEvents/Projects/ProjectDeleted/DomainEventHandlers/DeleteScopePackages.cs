namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents.Projects.ProjectDeleted.DomainEventHandlers;

public class DeleteScopePackages(TemplateDbContext templateDbContext) : IDomainEventHandler<ProjectDeleted>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task Handle(ProjectDeleted domainEvent)
    {
        var scopePackages = await _templateDbContext.ScopePackage
            .Where(x => x.ProjectId == domainEvent.Project.ProjectId && !x.IsDeleted)
            .ToListAsync();
        scopePackages.ForEach(x => x.SoftDelete());

        Console.WriteLine("Scope Packages Deleted");
    }
}