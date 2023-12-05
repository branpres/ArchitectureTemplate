namespace ArchitectureTemplate.Application.UseCases.Projects.GetById;

public record GetProjectByIdResponse : CreateProjectResponse
{
    public GetProjectByIdResponse(Guid ProjectId, Guid CompanyId, string ProjectName, string? ProjectIdentifier, List<CreateProjectUserResponse>? ProjectUsers)
        : base(ProjectId, CompanyId, ProjectName, ProjectIdentifier, ProjectUsers)
    {
    }
}