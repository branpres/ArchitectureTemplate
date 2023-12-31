﻿namespace ArchitectureTemplate.Application.UseCases.BOMs.GetByProjectId;

internal class GetBillOfMaterialsByProjectIdRequestHandler(TemplateDbContext templateDbContext) : IRequestHandler<GetBillOfMaterialsByProjectIdRequest, GetBillOfMaterialsByProjectIdResponse>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task<Result<GetBillOfMaterialsByProjectIdResponse>> Handle(GetBillOfMaterialsByProjectIdRequest request, CancellationToken cancellationToken)
    {
        var billOfMaterials = await _templateDbContext.BillOfMaterials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProjectId == request.ProjectId && !x.IsDeleted, cancellationToken);

        if (billOfMaterials == null)
        {
            return new Result<GetBillOfMaterialsByProjectIdResponse>(new NotFoundResultProblem());
        }

        return new Result<GetBillOfMaterialsByProjectIdResponse>(billOfMaterials.MapToGetBillOfMaterialsByProjectIdResponse());
    }
}