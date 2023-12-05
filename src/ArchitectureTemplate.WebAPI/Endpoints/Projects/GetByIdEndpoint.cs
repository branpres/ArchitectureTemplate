namespace ArchitectureTemplate.WebAPI.Endpoints.Projects;

public class GetByIdEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/project/{id}", Get)
            .WithOpenApi(x => new(x)
            {
                OperationId = "GetProjectById",
                Description = "Gets a project by id."
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return builder;
    }

    private async Task<IResult> Get(
        Guid id,
        IRequestHandler<Guid, GetProjectByIdResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, cancellationToken);

        return result.Match(
            getProjectByIdResponse => Ok(getProjectByIdResponse!),
            notFoundException => Results.NotFound());
    }

    private static IResult Ok(GetProjectByIdResponse createProjectResponse)
    {
        var links = new List<Link>
        {
            { new Link("DeleteProject", $"/project/{createProjectResponse.ProjectId}", HttpMethod.Delete.ToString()) },
            { new Link("BillOfMaterialsByProjectId", $"/bom/{createProjectResponse.ProjectId}", HttpMethod.Get.ToString()) },
            { new Link("ScopeByProjectId", $"/scopepackage/{createProjectResponse.ProjectId}", HttpMethod.Get.ToString()) }
        };

        return Results.Ok(createProjectResponse.Map(links));
    }
}