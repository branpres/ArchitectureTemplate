namespace ArchitectureTemplate.WebAPI.Endpoints.Projects;

public class CreateEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/project", Create)
            .WithOpenApi(x => new(x)
            {
                OperationId = "CreateProject",
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
            resultException => Results.BadRequest(
                resultException.Errors != null
                    ? new HttpValidationProblemDetails(resultException.Errors)
                    : new HttpValidationProblemDetails()));
    }

    private static IResult Created(CreateProjectResponse createProjectResponse)
    {
        var links = new List<Link>
        {
            { new Link("GetById", $"/project/{createProjectResponse.ProjectId}", HttpMethod.Get.ToString()) },
            { new Link("Delete", $"/project/{createProjectResponse.ProjectId}", HttpMethod.Delete.ToString()) }
        };

        return Results.Created($"/project/{createProjectResponse.ProjectId}", createProjectResponse.Map(links));
    }
}