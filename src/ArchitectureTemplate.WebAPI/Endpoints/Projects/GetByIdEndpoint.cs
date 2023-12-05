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
            getProjectByIdResponse => Results.Ok(getProjectByIdResponse!),
            notFoundException => Results.NotFound());
    }
}