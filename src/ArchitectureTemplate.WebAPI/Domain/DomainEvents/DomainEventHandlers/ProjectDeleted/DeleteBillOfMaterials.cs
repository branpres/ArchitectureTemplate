namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents.DomainEventHandlers.ProjectDeleted;

internal class DeleteBillOfMaterials(TemplateDbContext templateDbContext) : IDomainEventHandler<ProjectDeletedDomainEvent>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task Handle(ProjectDeletedDomainEvent domainEvent)
    {
        var billOfMaterials = await _templateDbContext.BillOfMaterials
            .FirstOrDefaultAsync(x => x.ProjectId == domainEvent.Project.ProjectId && !x.IsDeleted);
        billOfMaterials?.SoftDelete();

        Console.WriteLine("Bill Of Materials Deleted");
    }
}