﻿namespace ArchitectureTemplate.WebAPI.Projects;

public class CreateEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/project", CreateProject)
        .WithOpenApi(x => new(x)
        {
            OperationId = "Create Project",
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
        var response = await handler.Handle(request, cancellationToken);
        return Results.Created("", response);
    }
}