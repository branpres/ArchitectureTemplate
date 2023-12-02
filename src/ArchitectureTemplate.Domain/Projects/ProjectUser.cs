using ArchitectureTemplate.Domain.DomainEvents;
using ArchitectureTemplate.Domain.Interfaces;

namespace ArchitectureTemplate.Domain.Projects;

public class ProjectUser : DomainEventEntityBase, IBasicMetadata, IDeleteMetadata
{
    public Guid ProjectUserId { get; set; }

    public Guid ProjectId { get; set; }

    public required Project Project { get; set; }

    public Guid UserId { get; set; }

    public bool IsAdmin { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}