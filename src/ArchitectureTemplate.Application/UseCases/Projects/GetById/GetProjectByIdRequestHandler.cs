namespace ArchitectureTemplate.Application.UseCases.Projects.GetById;

public class GetProjectByIdRequestHandler(TemplateDbContext templateDbContext) : IRequestHandler<Guid, GetProjectByIdResponse>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task<Result<GetProjectByIdResponse?>> Handle(Guid id, CancellationToken cancellationToken)
    {
        var project = await _templateDbContext.Project
            .AsNoTracking()
            .Include(x => x.ProjectUsers)
            .Where(x => x.ProjectId == id && !x.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken);

        if (project == null)
        {
            return new Result<GetProjectByIdResponse?>(new NotFoundException());
        }

        return new Result<GetProjectByIdResponse?>(project.MapToGetProjectByIdResponse());
    }
}