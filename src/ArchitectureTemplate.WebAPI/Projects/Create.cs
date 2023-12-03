namespace ArchitectureTemplate.WebAPI.Projects;

public class CreateEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/project", CreateProject)
        .WithOpenApi(x => new(x)
        {
            OperationId = "CreateProject",
            Description = "Creates a new project. Assigns first project user. Notifies new project user. Creates initial project scope package. Creates project BOM."
        })
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem();

        return builder;
    }

    private async Task<IResult> CreateProject(
        CreateProjectRequest request,
        [FromServices] IRequestHandler<CreateProjectRequest, CreateProjectResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);
        return result.Match(
            response => Results.Created("", response),
            exception => Results.BadRequest());
    }
}