namespace ArchitectureTemplate.WebAPI.Features.BOMs;

public class GetByProjectIdEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/bom", Get)
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

    private static async Task<IResult> Get(
        Guid projectId,
        TemplateDbContext templateDbContext,
        CancellationToken cancellationToken)
    {
        var handler = new GetByProjectIdHandler(templateDbContext);
        var result = await handler.Handle(projectId, cancellationToken);

        return result.Match(
            Results.Ok,
            resultProblem => resultProblem is NotFoundResultProblem
                ? Results.NotFound()
                : Results.BadRequest(
                    resultProblem.Errors.Count > 0
                    ? new HttpValidationProblemDetails(resultProblem.Errors)
                    : new HttpValidationProblemDetails()));
    }
}

public class GetByProjectIdHandler(TemplateDbContext templateDbContext) : IRequestHandler<Guid, GetByProjectIdResponse>
{
    public async Task<Result<GetByProjectIdResponse>> Handle(Guid projectId, CancellationToken cancellationToken)
    {
        var billOfMaterials = await templateDbContext.BillOfMaterials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProjectId == projectId && !x.IsDeleted, cancellationToken);

        return billOfMaterials == null
            ? new Result<GetByProjectIdResponse>(new NotFoundResultProblem())
            : new Result<GetByProjectIdResponse>(billOfMaterials.MapToGetByProjectIdResponse());
    }
}

public record GetByProjectIdResponse(
    Guid BillOfMaterialsId,
    Guid ProjectId,
    string BillOfMaterialsName);

public static class GetByProjectIdResponseMapper
{
    public static GetByProjectIdResponse MapToGetByProjectIdResponse(this BillOfMaterials billOfMaterials)
    {
        return new GetByProjectIdResponse(
            billOfMaterials.BillOfMaterialsId,
            billOfMaterials.ProjectId,
            billOfMaterials.BillOfMaterialsName);
    }
}