namespace ArchitectureTemplate.Application.UseCases.Projects.Delete;

public class DeleteProjectRequestHandler(TemplateDbContext templateDbContext) : IRequestHandler<Guid>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task<Result> Handle(Guid projectId, CancellationToken cancellationToken)
    {
        var project = await _templateDbContext.Project.GetProjectWithProjectUsers(projectId, cancellationToken);

        if (project == null)
        {
            return new Result(new NotFoundResultProblem());
        }

        project.SoftDelete();

        return new Result();
    }
}