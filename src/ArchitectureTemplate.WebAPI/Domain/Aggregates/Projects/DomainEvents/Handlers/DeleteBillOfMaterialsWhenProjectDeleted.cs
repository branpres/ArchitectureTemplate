namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents.Handlers;

public class DeleteBillOfMaterialsWhenProjectDeleted(TemplateDbContext templateDbContext) : IDomainEventHandler<ProjectDeleted>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task Handle(ProjectDeleted domainEvent)
    {
        var billOfMaterials = await _templateDbContext.BillOfMaterials
            .FirstOrDefaultAsync(x => x.ProjectId == domainEvent.Project.ProjectId && !x.IsDeleted);
        billOfMaterials?.SoftDelete();

        Console.WriteLine("Bill Of Materials Deleted");
    }
}