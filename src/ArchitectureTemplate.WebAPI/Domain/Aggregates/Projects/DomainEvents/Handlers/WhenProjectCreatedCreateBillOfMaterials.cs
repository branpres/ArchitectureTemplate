namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects.DomainEvents.Handlers;

public class WhenProjectCreatedCreateBillOfMaterials(TemplateDbContext templateDbContext) : IDomainEventHandler
{
    public async Task Handle(IDomainEvent domainEvent)
    {
        if (domainEvent is ProjectCreated projectCreated)
        {
            var billOfMaterials = new BillOfMaterials
            {
                ProjectId = projectCreated.Project.ProjectId,
                BillOfMaterialsName = projectCreated.Project.ProjectName!
            };

            await templateDbContext.BillOfMaterials.AddAsync(billOfMaterials);

            Console.WriteLine("Bill Of Materials Created");
        }
    }
}