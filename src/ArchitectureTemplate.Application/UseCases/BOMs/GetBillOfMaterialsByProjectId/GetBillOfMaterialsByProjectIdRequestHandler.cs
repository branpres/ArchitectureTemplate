namespace ArchitectureTemplate.Application.UseCases.BOMs.GetBillOfMaterialsByProjectId;

internal class GetBillOfMaterialsByProjectIdRequestHandler(TemplateDbContext templateDbContext) : IRequestHandler<Guid, GetBillOfMaterialsByProjectIdResponse>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task<Result<GetBillOfMaterialsByProjectIdResponse?>> Handle(Guid projectId, CancellationToken cancellationToken)
    {
        var billOfMaterials = await _templateDbContext.BillOfMaterials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProjectId == projectId && !x.IsDeleted, cancellationToken);

        if (billOfMaterials == null)
        {
            return new Result<GetBillOfMaterialsByProjectIdResponse?>(new NotFoundException());
        }

        return new Result<GetBillOfMaterialsByProjectIdResponse?>(billOfMaterials.MapToResponse());
    }
}