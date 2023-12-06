namespace ArchitectureTemplate.WebAPI.Endpoints.BOMs;

public class GetByProjectIdEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/bom/{id}", Get)
            .WithOpenApi(x => new(x)
            {
                OperationId = "GetBillOfMaterialsByProjectId",
                Tags = new List<OpenApiTag> { new() { Name = "Bill of Materials" } },
                Description = "Gets a bill of materials by project id."
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        return builder;
    }

    private async Task<IResult> Get(
        Guid id,
        IRequestHandler<Guid, GetBillOfMaterialsByProjectIdResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(id, cancellationToken);

        return result.Match(
            getBillOfMaterialsByProjectIdResponse => Results.Ok(getBillOfMaterialsByProjectIdResponse!.Map()),
            resultProblem => resultProblem is NotFoundResultProblem
                ? Results.NotFound()
                : Results.BadRequest(
                    resultProblem.Errors.Count > 0
                    ? new HttpValidationProblemDetails(resultProblem.Errors)
                    : new HttpValidationProblemDetails()));
    }
}