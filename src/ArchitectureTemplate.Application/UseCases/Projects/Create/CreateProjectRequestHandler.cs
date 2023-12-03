

namespace ArchitectureTemplate.Application.UseCases.Projects.Create;

public class CreateProjectRequestHandler : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
{
    Task<Result<CreateProjectResponse?>> IRequestHandler<CreateProjectRequest, CreateProjectResponse>.Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var response = new CreateProjectResponse(Guid.NewGuid(), request.CompanyId, request.ProjectName, request.ProjectIdentifier);
        var result = new Result<CreateProjectResponse?>(response);
        return Task.FromResult(result);
    }
}