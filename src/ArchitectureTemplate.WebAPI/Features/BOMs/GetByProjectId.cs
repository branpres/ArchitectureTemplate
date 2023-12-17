namespace ArchitectureTemplate.WebAPI.Features.BOMs;

public static class GetByProjectId
{
    public class Endpoint : IEndpoint
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
            Guid projectId,
            TemplateDbContext templateDbContext,
            CancellationToken cancellationToken)
        {
            var handler = new Handler(templateDbContext);
            var result = await handler.Handle(projectId, cancellationToken);

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

    public class Handler(TemplateDbContext templateDbContext) : IRequestHandler<Guid, GetBomByProjectIdResponse>
    {
        private readonly TemplateDbContext _templateDbContext = templateDbContext;

        public async Task<Result<GetBomByProjectIdResponse>> Handle(Guid projectId, CancellationToken cancellationToken)
        {
            var billOfMaterials = await _templateDbContext.BillOfMaterials
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProjectId == projectId && !x.IsDeleted, cancellationToken);

            if (billOfMaterials == null)
            {
                return new Result<GetBomByProjectIdResponse>(new NotFoundResultProblem());
            }

            return new Result<GetBomByProjectIdResponse>(billOfMaterials.MapToGetByProjectIdResponse());
        }
    }

    public record GetBomByProjectIdResponse(
        Guid BillOfMaterialsId,
        Guid ProjectId,
        string BillOfMaterialsName);    
}

public static class GetByProjectIdMapper
{
    public static GetByProjectId.GetBomByProjectIdResponse MapToGetByProjectIdResponse(this BillOfMaterials billOfMaterials)
    {
        return new GetByProjectId.GetBomByProjectIdResponse(
            billOfMaterials.BillOfMaterialsId,
            billOfMaterials.ProjectId,
            billOfMaterials.BillOfMaterialsName);
    }
}