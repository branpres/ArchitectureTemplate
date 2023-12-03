namespace ArchitectureTemplate.Application.UseCases.Projects.Create;

public record CreateProjectRequest(
    Guid CompanyId,
    string ProjectName,
    string? ProjectIdentifier,
    Guid? ProjectTypeId,
    Guid? AdminUserId);