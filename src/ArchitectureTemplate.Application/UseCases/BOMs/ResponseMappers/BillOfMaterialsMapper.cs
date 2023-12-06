namespace ArchitectureTemplate.Application.UseCases.BOMs.ResponseMappers;

internal static class BillOfMaterialsMapper
{
    public static GetBillOfMaterialsByProjectIdResponse MapToResponse(this BillOfMaterials billOfMaterials)
    {
        return new GetBillOfMaterialsByProjectIdResponse(
            billOfMaterials.BillOfMaterialsId,
            billOfMaterials.ProjectId,
            billOfMaterials.BillOfMaterialsName);
    }
}