namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents.DomainEventHandlers.ProjectCreated;

internal class CreateBillOfMaterials(TemplateDbContext templateDbContext) : IDomainEventHandler<ProjectCreatedDomainEvent>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task Handle(ProjectCreatedDomainEvent domainEvent)
    {
        var billOfMaterials = new BillOfMaterials
        {
            ProjectId = domainEvent.Project.ProjectId,
            BillOfMaterialsName = domainEvent.Project.ProjectName!
        };

        await _templateDbContext.BillOfMaterials.AddAsync(billOfMaterials);

        Console.WriteLine("Bill Of Materials Created");
    }
}