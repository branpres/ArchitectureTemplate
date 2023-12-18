namespace ArchitectureTemplate.WebAPI.Features.LocationBreakdownStructure;

public class GetScopePackagesByProjectIdEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/scopepackage", Get)
            .WithOpenApi(x => new(x)
            {
                OperationId = "GetScopePackageByProjectId",
                Tags = new List<OpenApiTag> { new() { Name = "Scope Packages" } },
                Description = "Gets a list of scope packages by project id."
            })
            .Produces(StatusCodes.Status200OK);

        return builder;
    }

    private async Task<IResult> Get(
        Guid projectId,
        TemplateDbContext templateDbContext,
        CancellationToken cancellationToken)
    {
        var handler = new GetScopePackagesByProjectIdHandler(templateDbContext);
        var result = await handler.Handle(projectId, cancellationToken);

        return result.Match(
            getScopePackageByProjectIdResponse => Results.Ok(getScopePackageByProjectIdResponse),
            resultProblem => resultProblem.Errors.Count > 0
                ? Results.BadRequest(new HttpValidationProblemDetails(resultProblem.Errors))
                : Results.BadRequest());
    }
}

public class GetScopePackagesByProjectIdHandler(TemplateDbContext templateDbContext) : IRequestHandler<Guid, List<GetScopePackagesByProjectIdResponse>>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task<Result<List<GetScopePackagesByProjectIdResponse>>> Handle(Guid projectId, CancellationToken cancellationToken)
    {
        var scopePackages = await _templateDbContext.ScopePackage
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId && !x.IsDeleted)
            .ToListAsync(cancellationToken);

        return new Result<List<GetScopePackagesByProjectIdResponse>>(scopePackages.MapToGetByProjectIdResponse());
    }
}

public record GetScopePackagesByProjectIdResponse(
    Guid ScopePackageId,
    Guid ProjectId,
    string ScopePackageName);

public static class GetByProjectIdResponseMapper
{
    public static List<GetScopePackagesByProjectIdResponse> MapToGetByProjectIdResponse(this List<ScopePackage> scopePackages)
    {
        return scopePackages.Select(x => new GetScopePackagesByProjectIdResponse(x.ScopePackageId, x.ProjectId, x.ScopePackageName)).ToList();
    }
}