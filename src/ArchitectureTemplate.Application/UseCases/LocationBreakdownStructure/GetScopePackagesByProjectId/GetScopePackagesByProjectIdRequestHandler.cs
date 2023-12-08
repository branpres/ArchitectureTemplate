namespace ArchitectureTemplate.Application.UseCases.LocationBreakdownStructure.GetScopePackagesByProjectId;

internal class GetScopePackagesByProjectIdRequestHandler(TemplateDbContext templateDbContext) : IRequestHandler<Guid, List<GetScopePackagesByProjectIdResponse>>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task<Result<List<GetScopePackagesByProjectIdResponse>>> Handle(Guid projectId, CancellationToken cancellationToken)
    {
        var scopePackages = await _templateDbContext.ScopePackage
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId && !x.IsDeleted)
            .ToListAsync(cancellationToken);

        return new Result<List<GetScopePackagesByProjectIdResponse>>(scopePackages.MapToGetScopePackagesByProjectIdResponse());
    }
}