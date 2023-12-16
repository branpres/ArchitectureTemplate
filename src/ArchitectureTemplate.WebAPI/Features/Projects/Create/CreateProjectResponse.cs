namespace ArchitectureTemplate.WebAPI.Features.Projects.Create;

public record CreateProjectResponse(
    Guid ProjectId,
    Guid CompanyId,
    string ProjectName,
    string? ProjectIdentifier,
    List<CreateProjectUserResponse>? ProjectUsers);

internal static class Mapper
{
    public static CreateProjectResponse MapToCreateProjectResponse(this Project project)
    {
        return new CreateProjectResponse(
            project.ProjectId,
            project.CompanyId,
            project.ProjectName!,
            project.ProjectIdentifier,
            project.MapToCreateProjectUserResponses());
    }
}