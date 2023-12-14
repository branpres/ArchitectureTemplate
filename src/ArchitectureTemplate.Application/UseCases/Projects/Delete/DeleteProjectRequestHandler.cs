namespace ArchitectureTemplate.Application.UseCases.Projects.Delete;

internal class DeleteProjectRequestHandler(TemplateDbContext templateDbContext) : IRequestHandler<DeleteProjectRequest>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task<Result> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        var project = await _templateDbContext.Project.GetProjectWithProjectUsers(request.ProjectId, cancellationToken);

        if (project == null)
        {
            return new Result(new NotFoundResultProblem());
        }

        project.SoftDelete();
        await _templateDbContext.SaveChangesAsync(cancellationToken);

        return new Result();
    }
}