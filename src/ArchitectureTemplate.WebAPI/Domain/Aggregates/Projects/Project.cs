namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects;

internal class Project : DomainEventEntityBase, IBasicMetadata, IDeleteMetadata
{
    private Project() { }

    private Project(Guid companyId, string projectName, Guid? projectTypeId, string? projectIdentifier)
    {
        CompanyId = companyId;
        ProjectName = projectName;
        ProjectTypeId = projectTypeId;
        ProjectIdentifier = projectIdentifier;
    }

    public Guid ProjectId { get; private set; }

    public Guid CompanyId { get; private set; }

    public string? ProjectName { get; private set; }

    public Guid? ProjectTypeId { get; private set; }

    public string? ProjectIdentifier { get; private set; }

    public List<ProjectUser> ProjectUsers { get; private set; } = [];

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public static Project Create(Guid companyId, string projectName, Guid? projectTypeId = null, string? projectIdentifier = null)
    {
        var project = new Project(companyId, projectName, projectTypeId, projectIdentifier);
        project.RegisterDomainEvent(new ProjectCreatedDomainEvent(project));

        return project;
    }

    public void AddProjectUser(Guid userId)
    {
        var projectUser = ProjectUser.CreateNonAdminUser(this, userId);
        projectUser.RegisterDomainEvent(new ProjectUserAddedDomainEvent(projectUser));

        ProjectUsers.Add(projectUser);
    }

    public void AddProjectAdminUser(Guid userId)
    {
        var projectUser = ProjectUser.CreateAdminUser(this, userId);
        projectUser.RegisterDomainEvent(new ProjectUserAddedDomainEvent(projectUser));
        projectUser.RegisterDomainEvent(new ProjectAdminUserAddedDomainEvent(projectUser));

        ProjectUsers.Add(projectUser);
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        ProjectUsers.ForEach(x => x.SoftDelete());

        RegisterDomainEvent(new ProjectDeletedDomainEvent(this));
    }
}