namespace ArchitectureTemplate.WebAPI.Features.Projects;

public class GetProjectByIdEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/project/{projectId}", Get)
            .WithOpenApi(x => new(x)
            {
                OperationId = "GetProjectById",
                Tags = new List<OpenApiTag> { new() { Name = "Projects" } },
                Description = "Gets a project by id."
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
        var handler = new GetProjectByIdHandler(templateDbContext);
        var result = await handler.Handle(projectId, cancellationToken);

        return result.Match(
            getProjectByIdResponse => Ok(getProjectByIdResponse!),
            resultProblem => resultProblem is NotFoundResultProblem
                ? Results.NotFound()
                : Results.BadRequest(
                    resultProblem.Errors.Count > 0
                    ? new HttpValidationProblemDetails(resultProblem.Errors)
                    : new HttpValidationProblemDetails()));
    }

    private static IResult Ok(GetProjectByIdResponse getProjectByIdResponse)
    {
        getProjectByIdResponse.Links = new List<Link>
        {
            { new("DeleteProject", $"/project/{getProjectByIdResponse.ProjectId}", HttpMethod.Delete.ToString()) },
            { new("GetBillOfMaterialsByProjectId", $"/bom?projectId={getProjectByIdResponse.ProjectId}", HttpMethod.Get.ToString()) },
            { new("GetScopePackagesByProjectId", $"/scopepackage?projectId={getProjectByIdResponse.ProjectId}", HttpMethod.Get.ToString()) }
        };

        return Results.Ok(getProjectByIdResponse);
    }
}

public class GetProjectByIdHandler(TemplateDbContext templateDbContext) : IRequestHandler<Guid, GetProjectByIdResponse>
{
    public async Task<Result<GetProjectByIdResponse>> Handle(Guid projectId, CancellationToken cancellationToken)
    {
        var project = await templateDbContext.Project
            .AsNoTracking()
            .GetProjectWithProjectUsers(projectId, cancellationToken);

        return project == null
            ? new Result<GetProjectByIdResponse>(new NotFoundResultProblem())
            : new Result<GetProjectByIdResponse>(project.MapToGetByIdResponse());
    }
}

public record GetProjectByIdResponse(
    Guid ProjectId,
    Guid CompanyId,
    string ProjectName,
    string? ProjectIdentifier,
    List<CreateProjectUserResponse>? ProjectUsers)
    : ResponseBase;

public static class GetProjectByIdResponseMapper
{
    public static GetProjectByIdResponse MapToGetByIdResponse(this Project project)
    {
        return new GetProjectByIdResponse(
            project.ProjectId,
            project.CompanyId,
            project.ProjectName!,
            project.ProjectIdentifier,
            project.MapToCreateUserResponses());
    }
}