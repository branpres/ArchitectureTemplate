using ArchitectureTemplate.Application.UseCases.Projects.CreateUser;

namespace ArchitectureTemplate.Application.UseCases.Projects.Create;

public record CreateProjectResponse(
    Guid ProjectId,
    Guid CompanyId,
    string ProjectName,
    string? ProjectIdentifier,
    List<CreateProjectUserResponse>? CreateProjectUserResponses);