namespace ArchitectureTemplate.Application.UseCases.Projects.Delete;

public class DeleteProjectRequestHandler(TemplateDbContext templateDbContext) : IRequestHandler<Guid>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task<Result> Handle(Guid id, CancellationToken cancellationToken)
    {
        var project = await _templateDbContext.Project
            .Include(x => x.ProjectUsers)
            .Where(x => x.ProjectId == id && !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        if (project == null)
        {
            return new Result(new NotFoundException());
        }

        return new Result();
    }
}