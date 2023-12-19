﻿namespace ArchitectureTemplate.WebAPI.Domain.Aggregates.Projects;

public class Project : DomainEventEntityBase, IBasicMetadata, IDeleteMetadata
{
    private Project() { }

    public Project(Guid companyId, string projectName, Guid? projectTypeId = null, string? projectIdentifier = null)
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