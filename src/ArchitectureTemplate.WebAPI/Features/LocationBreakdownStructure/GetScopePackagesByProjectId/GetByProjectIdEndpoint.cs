namespace ArchitectureTemplate.WebAPI.Features.LocationBreakdownStructure.GetScopePackagesByProjectId;

internal class GetByProjectIdEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/scopepackage/{projectId}", Get)
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
        [AsParameters] GetScopePackageByProjectIdRequest request,
        IRequestHandler<GetScopePackageByProjectIdRequest, List<GetScopePackagesByProjectIdResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        return result.Match(
            getScopePackageByProjectIdResponse => Results.Ok(getScopePackageByProjectIdResponse.Map()),
            resultProblem => resultProblem.Errors.Count > 0
                ? Results.BadRequest(new HttpValidationProblemDetails(resultProblem.Errors))
                : Results.BadRequest());
    }
}