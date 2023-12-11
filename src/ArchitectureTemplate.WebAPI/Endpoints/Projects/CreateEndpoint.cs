namespace ArchitectureTemplate.WebAPI.Endpoints.Projects;

internal class CreateEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/project", Create)
            .WithOpenApi(x => new(x)
            {
                OperationId = "CreateProject",
                Tags = new List<OpenApiTag> { new() { Name = "Projects" } },
                Description = "Creates a new project. Assigns first project user. Notifies new project user. Creates initial project scope package. Creates project BOM."
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem();

        return builder;
    }

    private async Task<IResult> Create(
        CreateProjectRequest request,
        IRequestHandler<CreateProjectRequest, CreateProjectResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        return result.Match(
            createProjectResponse => Created(createProjectResponse!),
            resultProblem => Results.BadRequest(
                resultProblem.Errors.Count > 0
                    ? new HttpValidationProblemDetails(resultProblem.Errors)
                    : new HttpValidationProblemDetails()));
    }

    private static IResult Created(CreateProjectResponse createProjectResponse)
    {
        var links = new List<Link>
        {
            { new Link("GetProjectById", $"/project/{createProjectResponse.ProjectId}", HttpMethod.Get.ToString()) },
            { new Link("DeleteProject", $"/project/{createProjectResponse.ProjectId}", HttpMethod.Delete.ToString()) },
            { new Link("GetBillOfMaterialsByProjectId", $"/bom/{createProjectResponse.ProjectId}", HttpMethod.Get.ToString()) },
            { new Link("GetScopeByProjectId", $"/scopepackage/{createProjectResponse.ProjectId}", HttpMethod.Get.ToString()) },
        };

        return Results.Created($"/project/{createProjectResponse.ProjectId}", createProjectResponse.Map(links));
    }
}