using ArchitectureTemplate.WebAPI.Features.Projects;
using ArchitectureTemplate.WebAPI.Shared;

namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects;

public class Project : DomainEventEntityBase, IBasicMetadata, IDeleteMetadata
{
    private Project() { }

    private Project(Guid companyId, string projectName, Guid? projectTypeId = null, string? projectIdentifier = null)
    {
        CompanyId = companyId;
        ProjectName = projectName;
        ProjectTypeId = projectTypeId;
        ProjectIdentifier = projectIdentifier;

        RegisterDomainEvent(new ProjectCreated(this));
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

    public static Project Create(CreateProjectRequest request, ICurrentUser currentUser)
    {
        var project = new Project(request.CompanyId, request.ProjectName, request.ProjectTypeId, request.ProjectIdentifier);

        if (request.AdminUserId.HasValue)
        {
            project.AddProjectAdminUser(request.AdminUserId.Value);
        }

        if ((request.AdminUserId.HasValue && request.AdminUserId.Value != currentUser.User!.UserId) || !request.AdminUserId.HasValue)
        {
            project.AddProjectUser(currentUser.User!.UserId);
        }

        return project;
    }

    public void AddProjectUser(Guid userId)
    {
        var projectUser = ProjectUser.CreateNonAdminUser(this, userId);
        projectUser.RegisterDomainEvent(new ProjectUserAdded(projectUser));

        ProjectUsers.Add(projectUser);
    }

    public void AddProjectAdminUser(Guid userId)
    {
        var projectUser = ProjectUser.CreateAdminUser(this, userId);
        projectUser.RegisterDomainEvent(new ProjectUserAdded(projectUser));
        projectUser.RegisterDomainEvent(new ProjectAdminUserAdded(projectUser));

        ProjectUsers.Add(projectUser);
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        ProjectUsers.ForEach(x => x.SoftDelete());

        RegisterDomainEvent(new ProjectDeleted(this));
    }
}