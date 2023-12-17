﻿namespace ArchitectureTemplate.WebAPI.Domain.DomainEvents.Projects.ProjectCreated.DomainEventHandlers;

public class CreateBillOfMaterials(TemplateDbContext templateDbContext) : IDomainEventHandler<ProjectCreated>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task Handle(ProjectCreated domainEvent)
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