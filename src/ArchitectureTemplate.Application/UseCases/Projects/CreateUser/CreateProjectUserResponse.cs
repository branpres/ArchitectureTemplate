namespace ArchitectureTemplate.Application.UseCases.Projects.CreateUser;

public record CreateProjectUserResponse(
    Guid ProjectUserId,
    Guid ProjectId,
    Guid UserId,
    bool IsAdmin);