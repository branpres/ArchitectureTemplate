namespace ArchitectureTemplate.Application.UseCases.Projects.Create;

public class CreateProjectRequestHandler : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
{
    public Task<CreateProjectResponse> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new CreateProjectResponse(Guid.NewGuid(), request.CompanyId, request.ProjectName, request.ProjectIdentifier));
    }
}