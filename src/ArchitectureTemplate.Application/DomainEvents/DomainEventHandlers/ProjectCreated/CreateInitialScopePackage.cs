namespace ArchitectureTemplate.Application.DomainEvents.DomainEventHandlers.ProjectCreated;

internal class CreateInitialScopePackage(TemplateDbContext templateDbContext) : IDomainEventHandler<ProjectCreatedDomainEvent>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task Handle(ProjectCreatedDomainEvent domainEvent)
    {
        var scopePackage = new ScopePackage
        {
            ProjectId = domainEvent.Project.ProjectId
        };

        await _templateDbContext.ScopePackage.AddAsync(scopePackage);

        Console.WriteLine("Scope Package Created");
    }
}