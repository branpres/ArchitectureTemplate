namespace ArchitectureTemplate.WebAPI.Features.Projects.GetById;

public record GetProjectByIdResponse(
    Guid ProjectId,
    Guid CompanyId,
    string ProjectName,
    string? ProjectIdentifier,
    List<CreateProjectUserResponse>? ProjectUsers) : ResponseBase;

internal static class Mapper
{
    public static GetProjectByIdResponse MapToGetProjectByIdResponse(this Project project)
    {
        return new GetProjectByIdResponse(
            project.ProjectId,
            project.CompanyId,
            project.ProjectName!,
            project.ProjectIdentifier,
            project.MapToCreateProjectUserResponses());
    }
}