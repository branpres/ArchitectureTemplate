namespace ArchitectureTemplate.Application.UseCases.Projects.GetById;

internal class GetProjectByIdRequestHandler(TemplateDbContext templateDbContext) : IRequestHandler<GetProjectByIdRequest, GetProjectByIdResponse>
{
    private readonly TemplateDbContext _templateDbContext = templateDbContext;

    public async Task<Result<GetProjectByIdResponse>> Handle(GetProjectByIdRequest request, CancellationToken cancellationToken)
    {
        var project = await _templateDbContext.Project
            .AsNoTracking()
            .GetProjectWithProjectUsers(request.ProjectId, cancellationToken);

        if (project == null)
        {
            return new Result<GetProjectByIdResponse>(new NotFoundResultProblem());
        }

        return new Result<GetProjectByIdResponse>(project.MapToGetProjectByIdResponse());
    }
}