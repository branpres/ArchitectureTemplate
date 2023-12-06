namespace ArchitectureTemplate.Application.UseCases.BOMs.GetByProjectId;

public record GetBillOfMaterialsByProjectIdResponse(
    Guid BillOfMaterialsId,
    Guid ProjectId,
    string BillOfMaterialsName);