namespace ArchitectureTemplate.Domain.Aggregates.BillOfMaterials;

public class BillOfMaterials : IBasicMetadata, IDeleteMetadata
{
    public Guid BillOfMaterialsId { get; set; }

    public Guid ProjectId { get; set; }

    public Project? Project { get; set; }

    public string BillOfMaterialsName { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}