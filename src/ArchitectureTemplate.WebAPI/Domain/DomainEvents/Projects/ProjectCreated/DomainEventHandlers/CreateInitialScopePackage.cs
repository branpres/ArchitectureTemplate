namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents.Projects.ProjectCreated.DomainEventHandlers;

public class CreateInitialScopePackage(TemplateDbContext templateDbContext) : IDomainEventHandler<ProjectCreated>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task Handle(ProjectCreated domainEvent)
    {
        var scopePackage = new ScopePackage
        {
            ProjectId = domainEvent.Project.ProjectId
        };

        await _templateDbContext.ScopePackage.AddAsync(scopePackage);

        Console.WriteLine("Scope Package Created");
    }
}