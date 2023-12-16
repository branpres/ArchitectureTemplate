namespace ArchitectureTemplate.WebAPI.Features.LocationBreakdownStructure.GetScopePackagesByProjectId;

internal class GetScopePackagesByProjectIdRequestHandler(TemplateDbContext templateDbContext) : IRequestHandler<GetScopePackageByProjectIdRequest, List<GetScopePackagesByProjectIdResponse>>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task<Result<List<GetScopePackagesByProjectIdResponse>>> Handle(GetScopePackageByProjectIdRequest request, CancellationToken cancellationToken)
    {
        var scopePackages = await _templateDbContext.ScopePackage
            .AsNoTracking()
            .Where(x => x.ProjectId == request.ProjectId && !x.IsDeleted)
            .ToListAsync(cancellationToken);

        return new Result<List<GetScopePackagesByProjectIdResponse>>(scopePackages.MapToGetScopePackagesByProjectIdResponse());
    }
}