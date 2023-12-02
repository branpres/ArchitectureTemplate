using ArchitectureTemplate.Domain.DomainEvents;
using ArchitectureTemplate.Domain.Interfaces;

namespace ArchitectureTemplate.Domain.Projects;

public class Project : DomainEventEntityBase, IBasicMetadata, IDeleteMetadata
{
    public Guid ProjectId { get; set; }

    public Guid CompanyId { get; set; }

    public Guid? ProjectTypeId { get; set; }

    public List<ProjectUser>? ProjectUsers { get; set; }

    public required string ProjectName { get; set; }

    public string? ProjectIdentifier { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}