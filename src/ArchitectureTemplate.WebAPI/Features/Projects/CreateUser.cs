namespace ArchitectureTemplate.WebAPI.Features.Projects;

public record CreateProjectUserResponse(
    Guid ProjectUserId,
    Guid ProjectId,
    Guid UserId,
    bool IsAdmin);

public static class CreateProjectUserResponseMapper
{
    public static List<CreateProjectUserResponse>? MapToCreateUserResponses(this Project project)
    {
        List<CreateProjectUserResponse>? createProjectUserResponses = null;
        if (project.ProjectUsers.Count != 0)
        {
            createProjectUserResponses = project.ProjectUsers
                .Select(x => new CreateProjectUserResponse(x.ProjectUserId, x.ProjectId, x.UserId, x.IsAdmin))
                .ToList();
        }

        return createProjectUserResponses;
    }
}