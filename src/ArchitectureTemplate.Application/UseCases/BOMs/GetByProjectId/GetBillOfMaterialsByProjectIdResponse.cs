namespace ArchitectureTemplate.Application.UseCases.BOMs.GetByProjectId;

public record GetBillOfMaterialsByProjectIdResponse(
    Guid BillOfMaterialsId,
    Guid ProjectId,
    string BillOfMaterialsName);

internal static class Mapper
{
    public static GetBillOfMaterialsByProjectIdResponse MapToGetBillOfMaterialsByProjectIdResponse(this BillOfMaterials billOfMaterials)
    {
        return new GetBillOfMaterialsByProjectIdResponse(
            billOfMaterials.BillOfMaterialsId,
            billOfMaterials.ProjectId,
            billOfMaterials.BillOfMaterialsName);
    }
}