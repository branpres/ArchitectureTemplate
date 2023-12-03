namespace ArchitectureTemplate.Domain.Projects;

public class Project(Guid companyId, string projectName, Guid? projectTypeId = null, string? projectIdentifier = null)
    : DomainEventEntityBase, IBasicMetadata, IDeleteMetadata
{
    public Guid ProjectId { get; private set; }

    public Guid CompanyId { get; private set; } = companyId;

    public string ProjectName { get; private set; } = projectName;

    public Guid? ProjectTypeId { get; private set; } = projectTypeId;

    public string? ProjectIdentifier { get; private set; } = projectIdentifier;

    public List<ProjectUser>? ProjectUsers { get; private set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public void AddProjectUser(Guid userId)
    {
        ProjectUsers ??= [];
        ProjectUsers.Add(ProjectUser.CreateNonAdminUser(this, userId));
    }

    public void AddProjectAdminUser(Guid userId)
    {
        ProjectUsers ??= [];
        ProjectUsers.Add(ProjectUser.CreateAdminUser(this, userId));
    }
}