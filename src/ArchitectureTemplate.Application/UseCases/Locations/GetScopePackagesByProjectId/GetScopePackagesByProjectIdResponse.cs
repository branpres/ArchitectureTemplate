namespace ArchitectureTemplate.Application.UseCases.Locations.GetScopePackagesByProjectId;

public record GetScopePackagesByProjectIdResponse(
    Guid ScopePackageId,
    Guid ProjectId,
    string ScopePackageName);