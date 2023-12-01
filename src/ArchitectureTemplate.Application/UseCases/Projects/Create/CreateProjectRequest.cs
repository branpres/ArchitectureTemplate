namespace ArchitectureTemplate.Application.UseCases.Projects.Create;

public record CreateProjectRequest(
    Guid CompanyId,
    string ProjectName,
    string ProjectIdentifier,
    Guid RequestedBy,
    Guid? ProjectImageId,
    Guid? ProjectTypeId,
    Guid? UserId,
    string UserEmail,
    bool? IsAdmin);