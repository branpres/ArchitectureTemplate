namespace ArchitectureTemplate.Application.UseCases.Projects.Create;

public class CreateProjectRequestHandler : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
{
    private readonly Guid _currentUserId = Guid.NewGuid(); // pretend current user

    Task<Result<CreateProjectResponse?>> IRequestHandler<CreateProjectRequest, CreateProjectResponse>.Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var project = new Project(request.CompanyId, request.ProjectName, request.ProjectTypeId, request.ProjectIdentifier);
        
        if (request.AdminUserId.HasValue)
        {
            project.AddProjectAdminUser(request.AdminUserId.Value);
        }

        if (request.AdminUserId.HasValue && request.AdminUserId.Value != _currentUserId)
        {
            project.AddProjectUser(_currentUserId);
        }

        var result = new Result<CreateProjectResponse?>(project.Map());
        return Task.FromResult(result);
    }
}