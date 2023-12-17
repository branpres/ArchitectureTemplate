namespace ArchitectureTemplate.WebAPI.Features.Projects;

public static class CreateUser
{
    public record CreateUserResponse(
        Guid ProjectUserId,
        Guid ProjectId,
        Guid UserId,
        bool IsAdmin);
}

internal static class CreateUserResponseMapper
{
    public static List<CreateUser.CreateUserResponse>? MapToCreateUserResponses(this Project project)
    {
        List<CreateUser.CreateUserResponse>? createProjectUserResponses = null;
        if (project.ProjectUsers.Count != 0)
        {
            createProjectUserResponses = project.ProjectUsers
                .Select(x => new CreateUser.CreateUserResponse(x.ProjectUserId, x.ProjectId, x.UserId, x.IsAdmin))
                .ToList();
        }

        return createProjectUserResponses;
    }
}