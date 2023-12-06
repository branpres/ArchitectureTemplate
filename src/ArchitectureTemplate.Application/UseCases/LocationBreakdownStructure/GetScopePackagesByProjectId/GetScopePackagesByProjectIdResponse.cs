namespace ArchitectureTemplate.Application.UseCases.LocationBreakdownStructure.GetScopePackagesByProjectId;

public record GetScopePackagesByProjectIdResponse(
    Guid ScopePackageId,
    Guid ProjectId,
    string ScopePackageName);