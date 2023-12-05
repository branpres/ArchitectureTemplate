namespace ArchitectureTemplate.Application.UseCases.Projects.ResponseMappers;

public static class ProjectMapper
{
    public static CreateProjectResponse MapToCreateProjectResponse(this Project project)
    {
        return new CreateProjectResponse(
            project.ProjectId,
            project.CompanyId,
            project.ProjectName!,
            project.ProjectIdentifier,
            MapCreateProjectUserResponses(project));
    }

    public static GetProjectByIdResponse MapToGetProjectByIdResponse(this Project project)
    {
        return new GetProjectByIdResponse(
            project.ProjectId,
            project.CompanyId,
            project.ProjectName!,
            project.ProjectIdentifier,
            MapCreateProjectUserResponses(project));
    }

    private static List<CreateProjectUserResponse>? MapCreateProjectUserResponses(Project project)
    {
        List<CreateProjectUserResponse>? createProjectUserResponses = null;
        if (project.ProjectUsers != null && project.ProjectUsers.Count != 0)
        {
            createProjectUserResponses = project.ProjectUsers
                .Select(x => new CreateProjectUserResponse(x.ProjectUserId, x.ProjectId, x.UserId, x.IsAdmin))
                .ToList();
        }

        return createProjectUserResponses;
    }
}