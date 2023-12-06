namespace ArchitectureTemplate.WebAPI.Endpoints.LocationBreakdownStructure;

public class GetByProjectIdEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/scopepackage/{id}", Get)
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
        Guid id,
        IRequestHandler<Guid, List<GetScopePackagesByProjectIdResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, cancellationToken);

        return result.Match(
            getScopePackageByProjectIdResponse => Results.Ok(getScopePackageByProjectIdResponse),
            resultProblem => resultProblem.Errors.Count > 0 
                ? Results.BadRequest(new HttpValidationProblemDetails(resultProblem.Errors))
                : Results.Ok());
    }
}