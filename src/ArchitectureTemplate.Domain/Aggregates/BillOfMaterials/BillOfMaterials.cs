namespace ArchitectureTemplate.Domain.Aggregates.BillOfMaterials;

public class BillOfMaterials()
{
    public Guid BillOfMaterialsId { get; set; }

    public Guid ProjectId { get; set; }

    public Project? Project { get; set; }

    public string BillOfMaterialsName { get; set; } = string.Empty;
}