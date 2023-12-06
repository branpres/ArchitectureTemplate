namespace ArchitectureTemplate.Application.UseCases.Projects.Create;

public class CreateProjectRequestHandler(
    IValidator<CreateProjectRequest> validator,
    ICurrentUser currentUser,
    TemplateDbContext templateDbContext)
    : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
{
    private readonly IValidator<CreateProjectRequest> _validator = validator;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    async Task<Result<CreateProjectResponse?>> IRequestHandler<CreateProjectRequest, CreateProjectResponse>.Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await Validate(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.Result!;
        }

        var project = Create(request);

        await _templateDbContext.Project.AddAsync(project, cancellationToken);
        await _templateDbContext.SaveChangesAsync(cancellationToken);

        var result = new Result<CreateProjectResponse?>(project.MapToCreateProjectResponse());
        return result;
    }

    private async Task<(bool IsValid, Result<CreateProjectResponse?>? Result)> Validate(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return (false, new Result<CreateProjectResponse?>(new ResultProblem(validationResult.Errors)));
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
            return (false, new Result<CreateProjectResponse?>(invalidCreateDataResultProblem));
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