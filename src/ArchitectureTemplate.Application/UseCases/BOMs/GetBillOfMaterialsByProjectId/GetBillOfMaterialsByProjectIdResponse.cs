namespace ArchitectureTemplate.Application.UseCases.BOMs.GetBillOfMaterialsByProjectId;

public record GetBillOfMaterialsByProjectIdResponse(
    Guid BillOfMaterialsId,
    Guid ProjectId,
    string BillOfMaterialsName);