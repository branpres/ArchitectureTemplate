namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents.Handlers;

public class WhenProjectDeletedDeleteBillOfMaterials(TemplateDbContext templateDbContext) : IDomainEventHandler<ProjectDeleted>
{
    public async Task Handle(ProjectDeleted domainEvent)
    {
        var billOfMaterials = await templateDbContext.BillOfMaterials
            .FirstOrDefaultAsync(x => x.ProjectId == domainEvent.Project.ProjectId && !x.IsDeleted);
        billOfMaterials?.SoftDelete();

        Console.WriteLine("Bill Of Materials Deleted");
    }
}