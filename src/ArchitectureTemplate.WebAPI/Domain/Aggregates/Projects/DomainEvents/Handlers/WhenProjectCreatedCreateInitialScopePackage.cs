namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents.Handlers;

public class WhenProjectCreatedCreateInitialScopePackage(TemplateDbContext templateDbContext) : IDomainEventHandler
{
    public async Task Handle(IDomainEvent domainEvent)
    {
        if (domainEvent is  ProjectCreated projectCreated)
        {
            var scopePackage = new ScopePackage
            {
                ProjectId = projectCreated.Project.ProjectId
            };

            await templateDbContext.ScopePackage.AddAsync(scopePackage);

            Console.WriteLine("Scope Package Created");
        }
    }
}