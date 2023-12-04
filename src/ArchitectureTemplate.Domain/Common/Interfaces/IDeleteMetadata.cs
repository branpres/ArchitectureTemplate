namespace ArchitectureTemplate.Domain.Common.Interfaces;

public interface IDeleteMetadata
{
    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}