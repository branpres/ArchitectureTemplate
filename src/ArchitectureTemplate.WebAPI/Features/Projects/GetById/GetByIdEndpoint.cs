namespace ArchitectureTemplate.WebAPI.Features.Projects.GetById;

internal class GetByIdEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/project/{projectId}", Get)
            .WithOpenApi(x => new(x)
            {
                OperationId = "GetProjectById",
                Tags = new List<OpenApiTag> { new() { Name = "Projects" } },
                Description = "Gets a project by id."
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return builder;
    }

    private async Task<IResult> Get(
        [AsParameters] GetProjectByIdRequest request,
        IRequestHandler<GetProjectByIdRequest, GetProjectByIdResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        return result.Match(
            getProjectByIdResponse => Ok(getProjectByIdResponse!),
            resultProblem => resultProblem is NotFoundResultProblem
                ? Results.NotFound()
                : Results.BadRequest(
                    resultProblem.Errors.Count > 0
                    ? new HttpValidationProblemDetails(resultProblem.Errors)
                    : new HttpValidationProblemDetails()));
    }

    private static IResult Ok(GetProjectByIdResponse getProjectByIdResponse)
    {
        var links = new List<Link>
        {
            { new Link("DeleteProject", $"/project/{getProjectByIdResponse.ProjectId}", HttpMethod.Delete.ToString()) },
            { new Link("GetBillOfMaterialsByProjectId", $"/bom/{getProjectByIdResponse.ProjectId}", HttpMethod.Get.ToString()) },
            { new Link("GetScopeByProjectId", $"/scopepackage/{getProjectByIdResponse.ProjectId}", HttpMethod.Get.ToString()) }
        };

        return Results.Ok(getProjectByIdResponse.Map(links));
    }
}