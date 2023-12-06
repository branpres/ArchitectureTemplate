namespace ArchitectureTemplate.Application.UseCases.Locations.ResponseMappers;

internal static class ScopePackageMapper
{
    public static List<GetScopePackagesByProjectIdResponse> MapToResponse(this List<ScopePackage> scopePackages)
    {
        return scopePackages.Select(x => new GetScopePackagesByProjectIdResponse(x.ScopePackageId, x.ProjectId, x.ScopePackageName)).ToList();
    }
}