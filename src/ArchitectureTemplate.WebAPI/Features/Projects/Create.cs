namespace ArchitectureTemplate.WebAPI.Features.Projects;

public record CreateProjectRequest(
    Guid CompanyId,
    string ProjectName,
    string? ProjectIdentifier = null,
    Guid? ProjectTypeId = null,
    Guid? AdminUserId = null);

public class CreateProjectEndpoint : IEndpoint
{
    public IEndpointRouteBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("/project", Create)
            .WithOpenApi(x => new(x)
            {
                OperationId = "CreateProject",
                Tags = new List<OpenApiTag> { new() { Name = "Projects" } },
                Description = "Creates a new project. Assigns first project user. Notifies new project user. Creates initial project scope package. Creates project BOM."
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem();

        return builder;
    }

    private async Task<IResult> Create(
        CreateProjectRequest request,
        IValidator<CreateProjectRequest> validator,
        ICurrentUser currentUser,
        TemplateDbContext templateDbContext,
        CancellationToken cancellationToken)
    {
        var handler = new CreateProjectHandler(validator, currentUser, templateDbContext);
        var result = await handler.Handle(request, cancellationToken);

        return result.Match(
            createProjectResponse => Created(createProjectResponse!),
            resultProblem => Results.BadRequest(
                resultProblem.Errors.Count > 0
                    ? new HttpValidationProblemDetails(resultProblem.Errors)
                    : new HttpValidationProblemDetails()));
    }

    private static IResult Created(CreateProjectResponse createProjectResponse)
    {
        createProjectResponse.Links = new List<Link>
            {
                { new Link("GetProjectById", $"/project/{createProjectResponse.ProjectId}", HttpMethod.Get.ToString()) },
                { new Link("DeleteProject", $"/project/{createProjectResponse.ProjectId}", HttpMethod.Delete.ToString()) },
                { new Link("GetBillOfMaterialsByProjectId", $"/bom?projectId={createProjectResponse.ProjectId}", HttpMethod.Get.ToString()) },
                { new Link("GetScopePackagesByProjectId", $"/scopepackage?projectId={createProjectResponse.ProjectId}", HttpMethod.Get.ToString()) },
            };

        return Results.Created($"/project/{createProjectResponse.ProjectId}", createProjectResponse);
    }
}

public class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.ProjectName)
            .NotEmpty()
            .MaximumLength(64);

        RuleFor(x => x.ProjectIdentifier)
            .MaximumLength(64);
    }
}

public class CreateProjectHandler(
        IValidator<CreateProjectRequest> validator,
        ICurrentUser currentUser,
        TemplateDbContext templateDbContext)
        : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
{
    private readonly IValidator<CreateProjectRequest> _validator = validator;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task<Result<CreateProjectResponse>> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await Validate(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.Result!;
        }

        var project = Create(request);

        await _templateDbContext.Project.AddAsync(project, cancellationToken);
        await _templateDbContext.SaveChangesAsync(cancellationToken);

        var result = new Result<CreateProjectResponse>(project.MapToCreateResponse());
        return result;
    }

    private async Task<(bool IsValid, Result<CreateProjectResponse>? Result)> Validate(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return (false, new Result<CreateProjectResponse>(new ResultProblem(validationResult.Errors)));
        }

        var invalidCreateDataResultProblem = new ResultProblem();

        if (!await _templateDbContext.Project.IsProjectNameAvailable(request.CompanyId, request.ProjectName, cancellationToken))
        {
            invalidCreateDataResultProblem.AddError("ProjectName", "Project name already exists for this company.");
        }

        if (request.ProjectIdentifier != null && !await _templateDbContext.Project.IsProjectIdentifierAvailable(request.CompanyId, request.ProjectIdentifier, cancellationToken))
        {
            invalidCreateDataResultProblem.AddError("ProjectIdentifier", "Project identifier already exists for this company.");
        }

        if (invalidCreateDataResultProblem.Errors.Count > 0)
        {
            return (false, new Result<CreateProjectResponse>(invalidCreateDataResultProblem));
        }

        return (true, null);
    }

    private Project Create(CreateProjectRequest request)
    {
        var project = Project.Create(request.CompanyId, request.ProjectName, request.ProjectTypeId, request.ProjectIdentifier);

        if (request.AdminUserId.HasValue)
        {
            project.AddProjectAdminUser(request.AdminUserId.Value);
        }

        if ((request.AdminUserId.HasValue && request.AdminUserId.Value != _currentUser.User!.UserId) || !request.AdminUserId.HasValue)
        {
            project.AddProjectUser(_currentUser.User!.UserId);
        }

        return project;
    }
}

public record CreateProjectResponse(
    Guid ProjectId,
    Guid CompanyId,
    string ProjectName,
    string? ProjectIdentifier,
    List<CreateProjectUserResponse>? ProjectUsers)
    : ResponseBase;

public static class CreateProjectResponseMapper
{
    public static CreateProjectResponse MapToCreateResponse(this Project project)
    {
        return new CreateProjectResponse(
            project.ProjectId,
            project.CompanyId,
            project.ProjectName!,
            project.ProjectIdentifier,
            project.MapToCreateUserResponses());
    }
}