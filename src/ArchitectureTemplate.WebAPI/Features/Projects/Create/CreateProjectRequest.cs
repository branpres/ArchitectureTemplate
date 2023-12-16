namespace ArchitectureTemplate.WebAPI.Features.Projects.Create;
public record CreateProjectRequest(
    Guid CompanyId,
    string ProjectName,
    string? ProjectIdentifier = null,
    Guid? ProjectTypeId = null,
    Guid? AdminUserId = null);