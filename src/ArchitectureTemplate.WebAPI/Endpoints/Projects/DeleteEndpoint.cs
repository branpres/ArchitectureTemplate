namespace ArchitectureTemplate.WebAPI.Endpoints.Projects;

public class DeleteEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapDelete("/project/{id}", Delete)
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
        Guid id,
        IRequestHandler<Guid> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, cancellationToken);

        return result.Match(
            Results.NoContent,
            notFoundException => Results.NotFound());
    }
}