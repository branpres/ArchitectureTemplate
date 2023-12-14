using ArchitectureTemplate.Application.UseCases.Projects.Delete;

namespace ArchitectureTemplate.WebAPI.Endpoints.Projects;

internal class DeleteEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapDelete("/project/{projectId}", Delete)
            .WithOpenApi(x => new(x)
            {
                OperationId = "DeleteProject",
                Tags = new List<OpenApiTag> { new() { Name = "Projects" } },
                Description = "Deletes a project."
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return builder;
    }

    private async Task<IResult> Delete(
        [AsParameters] DeleteProjectRequest request,
        IRequestHandler<DeleteProjectRequest> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        return result.Match(
            Results.NoContent,
            resultProblem => resultProblem is NotFoundResultProblem
                ? Results.NotFound()
                : Results.BadRequest(
                    resultProblem.Errors.Count > 0
                    ? new HttpValidationProblemDetails(resultProblem.Errors)
                    : new HttpValidationProblemDetails()));
    }
}