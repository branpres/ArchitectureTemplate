namespace ArchitectureTemplate.WebAPI.Features.Projects;

public static class GetById
{
    public class Endpoint : IEndpoint
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

        private async Task<IResult> Get(
            Guid projectId,
            TemplateDbContext templateDbContext,
            CancellationToken cancellationToken)
        {
            var handler = new Handler(templateDbContext);
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
                { new Link("DeleteProject", $"/project/{getProjectByIdResponse.ProjectId}", HttpMethod.Delete.ToString()) },
                { new Link("GetBillOfMaterialsByProjectId", $"/bom/{getProjectByIdResponse.ProjectId}", HttpMethod.Get.ToString()) },
                { new Link("GetScopePackagesByProjectId", $"/scopepackage/{getProjectByIdResponse.ProjectId}", HttpMethod.Get.ToString()) }
            };

            return Results.Ok(getProjectByIdResponse);
        }
    }

    public class Handler(TemplateDbContext templateDbContext) : IRequestHandler<Guid, GetProjectByIdResponse>
    {
        private readonly TemplateDbContext _templateDbContext = templateDbContext;

        public async Task<Result<GetProjectByIdResponse>> Handle(Guid projectId, CancellationToken cancellationToken)
        {
            var project = await _templateDbContext.Project
                .AsNoTracking()
                .GetProjectWithProjectUsers(projectId, cancellationToken);

            if (project == null)
            {
                return new Result<GetProjectByIdResponse>(new NotFoundResultProblem());
            }

            return new Result<GetProjectByIdResponse>(project.MapToGetByIdResponse());
        }
    }

    public record GetProjectByIdResponse(
        Guid ProjectId,
        Guid CompanyId,
        string ProjectName,
        string? ProjectIdentifier,
        List<CreateUser.CreateUserResponse>? ProjectUsers)
        : ResponseBase;
}

public static class GetByIdResponseMapper
{
    public static GetById.GetProjectByIdResponse MapToGetByIdResponse(this Project project)
    {
        return new GetById.GetProjectByIdResponse(
            project.ProjectId,
            project.CompanyId,
            project.ProjectName!,
            project.ProjectIdentifier,
            project.MapToCreateUserResponses());
    }
}