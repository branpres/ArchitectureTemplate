namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects;

internal class ProjectUser : DomainEventEntityBase, IBasicMetadata, IDeleteMetadata
{
    private ProjectUser() { }

    private ProjectUser(Project project, Guid userId, bool isAdmin = false)
    {
        Project = project;
        UserId = userId;
        IsAdmin = isAdmin;
    }

    public Guid ProjectUserId { get; private set; }

    public Guid ProjectId { get; private set; }

    public Project? Project { get; set; }

    public Guid UserId { get; private set; }

    public bool IsAdmin { get; private set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public static ProjectUser CreateAdminUser(Project project, Guid userId) => new(project, userId, true);

    public static ProjectUser CreateNonAdminUser(Project project, Guid userId) => new(project, userId);

    public void SoftDelete()
    {
        IsDeleted = true;
    }
}