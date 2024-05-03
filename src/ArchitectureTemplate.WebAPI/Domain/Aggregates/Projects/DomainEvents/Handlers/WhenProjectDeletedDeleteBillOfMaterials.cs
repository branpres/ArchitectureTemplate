namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents.Handlers;

public class WhenProjectDeletedDeleteBillOfMaterials(TemplateDbContext templateDbContext) : IDomainEventHandler
{
    public async Task Handle(IDomainEvent domainEvent)
    {
        if (domainEvent is ProjectDeleted projectDeleted)
        {
            var billOfMaterials = await templateDbContext.BillOfMaterials
                .FirstOrDefaultAsync(x => x.ProjectId == projectDeleted.Project.ProjectId && !x.IsDeleted);
            billOfMaterials?.SoftDelete();

            Console.WriteLine("Bill Of Materials Deleted");
        }
    }
}