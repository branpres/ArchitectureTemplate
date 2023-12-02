namespace ArchitectureTemplate.Domain.Interfaces;

public interface IBasicMetadata
{
    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }
}