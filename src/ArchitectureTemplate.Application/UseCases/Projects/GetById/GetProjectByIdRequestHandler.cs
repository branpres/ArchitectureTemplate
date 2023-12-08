namespace ArchitectureTemplate.Application.UseCases.Projects.GetById;

public class GetProjectByIdRequestHandler(TemplateDbContext templateDbContext) : IRequestHandler<Guid, GetProjectByIdResponse>
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

        return new Result<GetProjectByIdResponse>(project.MapToGetProjectByIdResponse());
    }
}