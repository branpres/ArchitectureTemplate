namespace ArchitectureTemplate.WebAPI.Features.BOMs.GetByProjectId;

internal class GetByProjectIdEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/bom/{projectId}", Get)
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
        [AsParameters] GetBillOfMaterialsByProjectIdRequest request,
        IRequestHandler<GetBillOfMaterialsByProjectIdRequest, GetBillOfMaterialsByProjectIdResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        return result.Match(
            getBillOfMaterialsByProjectIdResponse => Results.Ok(getBillOfMaterialsByProjectIdResponse),
            resultProblem => resultProblem is NotFoundResultProblem
                ? Results.NotFound()
                : Results.BadRequest(
                    resultProblem.Errors.Count > 0
                    ? new HttpValidationProblemDetails(resultProblem.Errors)
                    : new HttpValidationProblemDetails()));
    }
}