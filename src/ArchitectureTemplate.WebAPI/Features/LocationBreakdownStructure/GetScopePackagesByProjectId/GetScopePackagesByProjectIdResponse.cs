namespace ArchitectureTemplate.WebAPI.Features.LocationBreakdownStructure.GetScopePackagesByProjectId;

public record GetScopePackagesByProjectIdResponse(
    Guid ScopePackageId,
    Guid ProjectId,
    string ScopePackageName);

internal static class Mapper
{
    public static List<GetScopePackagesByProjectIdResponse> MapToGetScopePackagesByProjectIdResponse(this List<ScopePackage> scopePackages)
    {
        return scopePackages.Select(x => new GetScopePackagesByProjectIdResponse(x.ScopePackageId, x.ProjectId, x.ScopePackageName)).ToList();
    }
}